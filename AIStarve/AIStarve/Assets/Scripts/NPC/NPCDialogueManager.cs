using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SceneManagement;
using System.IO;

namespace NPCDialogue
{
    [System.Serializable]
    public class NPCAffinitySaveData
    {
        public List<NPCAffinityEntry> entries = new List<NPCAffinityEntry>();
    }

    [System.Serializable]
    public class NPCAffinityEntry
    {
        public string npcId;
        public int affinity;
        public HashSet<string> triggeredDialogues = new HashSet<string>();
    }

    public class NPCDialogueManager : MonoBehaviour
    {
        private static NPCDialogueManager _instance;
        public static NPCDialogueManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("NPCDialogueManager");
                    _instance = go.AddComponent<NPCDialogueManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        private NPCDialogueConfig _config;
        private HashSet<string> _playerTags = new HashSet<string>();
        private Dictionary<string, NPCDefinition> _npcMap = new Dictionary<string, NPCDefinition>();
        private Dictionary<string, List<DialogueDefinition>> _npcDialogues = new Dictionary<string, List<DialogueDefinition>>();
        private Dictionary<string, HashSet<string>> _triggeredDialogues = new Dictionary<string, HashSet<string>>();
        private Dictionary<string, int> _affinity = new Dictionary<string, int>();
        private string _activeNpcId;
        private string _activeDialogueId;
        private string _affinityFilePath;
        private bool _continuePressed;
        private bool _isInDialogue;

        private string AffinityFilePath
        {
            get
            {
                if (_affinityFilePath == null)
                    _affinityFilePath = Path.Combine(Application.persistentDataPath, "NPCAffinity.json");
                return _affinityFilePath;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            LoadConfig();
        }

        private void LoadConfig()
        {
            var textAsset = Resources.Load<TextAsset>("NPC/NPCDialogue");
            if (textAsset == null)
            {
                Debug.LogError("NPCDialogueManager: Failed to load NPCDialogue.json from Resources/NPC/");
                return;
            }

            _config = JsonUtility.FromJson<NPCDialogueConfig>(textAsset.text);
            if (_config == null)
            {
                Debug.LogError("NPCDialogueManager: Failed to parse NPCDialogue.json");
                return;
            }

            foreach (var npc in _config.npcs)
            {
                _npcMap[npc.npcId] = npc;
                _npcDialogues[npc.npcId] = npc.dialogues;
                _triggeredDialogues[npc.npcId] = new HashSet<string>();
                if (!_affinity.ContainsKey(npc.npcId))
                    _affinity[npc.npcId] = 0;
            }

            LoadAffinity();

            Debug.Log($"NPCDialogueManager: Loaded {_config.npcs.Count} NPCs with config");
        }

        private void LoadAffinity()
        {
            if (File.Exists(AffinityFilePath))
            {
                string json = File.ReadAllText(AffinityFilePath);
                var data = JsonUtility.FromJson<NPCAffinitySaveData>(json);
                if (data != null && data.entries != null)
                {
                    foreach (var entry in data.entries)
                    {
                        if (!string.IsNullOrEmpty(entry.npcId))
                        {
                            _affinity[entry.npcId] = Mathf.Clamp(entry.affinity, -10, 10);
                            if (entry.triggeredDialogues != null && entry.triggeredDialogues.Count > 0)
                            {
                                if (!_triggeredDialogues.ContainsKey(entry.npcId))
                                    _triggeredDialogues[entry.npcId] = new HashSet<string>();
                                foreach (var d in entry.triggeredDialogues)
                                    _triggeredDialogues[entry.npcId].Add(d);
                            }
                        }
                    }
                }
            }
        }

        private void SaveAffinity()
        {
            var data = new NPCAffinitySaveData();
            foreach (var kvp in _affinity)
            {
                var entry = new NPCAffinityEntry
                {
                    npcId = kvp.Key,
                    affinity = kvp.Value
                };
                if (_triggeredDialogues.TryGetValue(kvp.Key, out var triggered))
                    entry.triggeredDialogues = new HashSet<string>(triggered);
                data.entries.Add(entry);
            }
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(AffinityFilePath, json);
        }

        public int GetAffinity(string npcId)
        {
            return _affinity.TryGetValue(npcId, out int val) ? val : 0;
        }

        public void AddAffinity(string npcId, int delta)
        {
            if (!_affinity.ContainsKey(npcId))
                _affinity[npcId] = 0;
            _affinity[npcId] = Mathf.Clamp(_affinity[npcId] + delta, -10, 10);
            SaveAffinity();
        }

        private static bool CheckAffinityCondition(int affinity, string condition)
        {
            if (string.IsNullOrEmpty(condition))
                return true;
            condition = condition.Trim();
            if (condition.StartsWith(">=") && int.TryParse(condition.Substring(2), out int ge))
                return affinity >= ge;
            if (condition.StartsWith("<=") && int.TryParse(condition.Substring(2), out int le))
                return affinity <= le;
            if (condition.StartsWith(">") && int.TryParse(condition.Substring(1), out int gt))
                return affinity > gt;
            if (condition.StartsWith("<") && int.TryParse(condition.Substring(1), out int lt))
                return affinity < lt;
            if (condition.StartsWith("==") && int.TryParse(condition.Substring(2), out int eq))
                return affinity == eq;
            return true;
        }
        public List<NPCDefinition> GetAllNPCDefinitions()
        {
            return new List<NPCDefinition>(_npcMap.Values);
        }

        public NPCDefinition GetNPCDefinition(string npcId)
        {
            _npcMap.TryGetValue(npcId, out var npc);
            return npc;
        }

        public DialogueDefinition GetDialogue(string npcId, string dialogueId)
        {
            if (!_npcDialogues.TryGetValue(npcId, out var dialogues))
                return null;
            return dialogues.FirstOrDefault(d => d.dialogueId == dialogueId);
        }

        public List<DialogueDefinition> GetDialoguesByTrigger(string npcId, TriggerType triggerType, int triggerAreaId = 0)
        {
            if (!_npcDialogues.TryGetValue(npcId, out var dialogues))
                return new List<DialogueDefinition>();

            return dialogues.Where(d =>
            {
                if (d.triggerType != triggerType.ToString())
                    return false;

                if (triggerType == TriggerType.enter_area && d.triggerAreaId != triggerAreaId)
                    return false;

                if (d.firstTimeOnly && _triggeredDialogues[npcId].Contains(d.dialogueId))
                    return false;

                if (d.conditionTags.Any() && !d.conditionTags.All(t => _playerTags.Contains(t)))
                    return false;

                if (d.excludeTags.Any() && d.excludeTags.Any(t => _playerTags.Contains(t)))
                    return false;

                return true;
            }).ToList();
        }

        public bool HasTag(string tag)
        {
            return _playerTags.Contains(tag);
        }

        public void AddTag(string tag)
        {
            if (!string.IsNullOrEmpty(tag))
                _playerTags.Add(tag);
        }

        private void Update()
        {
            if (_isInDialogue && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)))
            {
                _continuePressed = true;
            }
        }

        public void TriggerDialogue(string npcId, string dialogueId)
        {
            if (_isInDialogue) return;

            PauseNPCPatrol(npcId);

            var dialogue = GetDialogue(npcId, dialogueId);
            if (dialogue == null)
            {
                Debug.LogWarning($"NPCDialogueManager: Dialogue {dialogueId} not found for NPC {npcId}");
                return;
            }

            StartCoroutine(ExecuteDialogueCoroutine(npcId, dialogue));
        }

        private IEnumerator ExecuteDialogueCoroutine(string npcId, DialogueDefinition dialogue)
        {
            _isInDialogue = true;
            _continuePressed = false;

            Debug.Log($"--- NPC Dialogue [{npcId}] ---");

            ExecuteActions(dialogue.onStartActions, npcId);

            foreach (var line in dialogue.conversations)
            {
                string linePrefix = line.type == "think" ? "[内心独白] " : "";
                string speakerName;
                if (line.speaker == "npc")
                    speakerName = GetNPCDefinition(npcId)?.npcName ?? npcId;
                else if (line.speaker == "player")
                    speakerName = "玩家";
                else
                {
                    var otherDef = GetNPCDefinition(line.speaker);
                    speakerName = otherDef?.npcName ?? line.speaker;
                }
                Debug.Log($"[{speakerName}] {linePrefix}{line.text}");

                ExecuteLineAnimation(line.animation, npcId);
                ExecuteActions(line.onSpeakActions, npcId);

                yield return WaitForContinue();
            }

            if (!string.IsNullOrEmpty(npcId) && !string.IsNullOrEmpty(dialogue.dialogueId))
            {
                if (!_triggeredDialogues.ContainsKey(npcId))
                    _triggeredDialogues[npcId] = new HashSet<string>();
                _triggeredDialogues[npcId].Add(dialogue.dialogueId);
            }

            ExecuteActions(dialogue.onEndActions, npcId);

            var affinity = GetAffinity(npcId);
            var validChoices = dialogue.choices
                .Where(c => CheckAffinityCondition(affinity, c.affinityCondition))
                .ToList();

            if (validChoices.Count > 0)
            {
                _activeNpcId = npcId;
                _activeDialogueId = dialogue.dialogueId;
                Debug.Log($"--- 选择 (好感度: {affinity}) ---");
                for (int i = 0; i < validChoices.Count; i++)
                {
                    Debug.Log($"[{i + 1}] {validChoices[i].text}");
                }
            }
            else
            {
                _activeNpcId = null;
                _activeDialogueId = null;
                ResumeNPCPatrol(npcId);
            }

            _isInDialogue = false;
        }

        private IEnumerator WaitForContinue()
        {
            _continuePressed = false;
            while (!_continuePressed)
            {
                yield return null;
            }
        }

        public void SelectChoice(string npcId, string dialogueId, int choiceIndex)
        {
            var dialogue = GetDialogue(npcId, dialogueId);
            if (dialogue == null || choiceIndex < 0 || choiceIndex >= dialogue.choices.Count)
            {
                Debug.LogWarning($"NPCDialogueManager: Invalid choice for dialogue {dialogueId}");
                return;
            }

            _activeNpcId = null;
            _activeDialogueId = null;

            var choice = dialogue.choices[choiceIndex];
            Debug.Log($"[玩家] {choice.text}");

            if (choice.affinityDelta != 0)
            {
                AddAffinity(npcId, choice.affinityDelta);
                Debug.Log($"[好感度] {GetNPCDefinition(npcId)?.npcName ?? npcId}: {_affinity[npcId] - choice.affinityDelta} → {_affinity[npcId]} ({choice.affinityDelta:+0;-0;0})");
            }

            foreach (var tag in choice.addTags)
                AddTag(tag);

            ExecuteActions(choice.onSelectActions, npcId);

            if (!string.IsNullOrEmpty(choice.nextDialogueId))
            {
                TriggerDialogue(npcId, choice.nextDialogueId);
            }
            else
            {
                ResumeNPCPatrol(npcId);
            }
        }

        public bool TrySelectChoice(int choiceIndex)
        {
            if (string.IsNullOrEmpty(_activeNpcId) || string.IsNullOrEmpty(_activeDialogueId))
                return false;

            var dialogue = GetDialogue(_activeNpcId, _activeDialogueId);
            if (dialogue == null) return false;

            var affinity = GetAffinity(_activeNpcId);
            var validChoices = dialogue.choices
                .Where(c => CheckAffinityCondition(affinity, c.affinityCondition))
                .ToList();

            if (choiceIndex < 0 || choiceIndex >= validChoices.Count)
                return false;

            var choice = validChoices[choiceIndex];
            int originalIndex = dialogue.choices.IndexOf(choice);
            SelectChoice(_activeNpcId, _activeDialogueId, originalIndex);
            return true;
        }

        private void ExecuteActions(List<DialogueAction> actions, string npcId)
        {
            foreach (var action in actions)
            {
                switch (action.action)
                {
                    case "log":
                        string logNpcName = GetNPCDefinition(npcId)?.npcName ?? npcId;
                        Debug.Log($"[{logNpcName}] {action.message}");
                        break;

                    case "move":
                        string targetNpcId = string.IsNullOrEmpty(action.npcId) ? npcId : action.npcId;
                        if (_activeNPCControllers.TryGetValue(targetNpcId, out var controller))
                        {
                            Vector3 targetPos;
                            if (action.targetType == "triggerArea")
                            {
                                var areaGO = GameObject.Find($"TargetArea_{action.triggerAreaId}");
                                if (areaGO != null)
                                    targetPos = areaGO.transform.position;
                                else
                                    targetPos = action.target;
                            }
                            else
                            {
                                targetPos = action.target;
                            }
                            controller.MoveToPosition(targetPos);
                        }
                        break;
                }
            }
        }

        private void ExecuteLineAnimation(string animName, string npcId)
        {
            if (string.IsNullOrEmpty(animName) || string.IsNullOrEmpty(npcId))
                return;

            if (!_activeNPCControllers.TryGetValue(npcId, out var controller))
                return;

            if (animName == "face_player")
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Vector2 dir = (Vector2)player.transform.position - (Vector2)controller.transform.position;
                    if (dir.sqrMagnitude > 0.01f)
                    {
                        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                        int animDir = 0;
                        if (angle < -135 || angle > 135) animDir = 3;
                        else if (angle < -45) animDir = 0;
                        else if (angle < 45) animDir = 1;
                        else animDir = 2;
                        controller.SetFaceDir(animDir);
                    }
                }
                return;
            }

            switch (animName)
            {
                case "face_down":  controller.SetFaceDir(0); break;
                case "face_right": controller.SetFaceDir(1); break;
                case "face_up":    controller.SetFaceDir(2); break;
                case "face_left":  controller.SetFaceDir(3); break;
            }
        }

        private void PauseNPCPatrol(string npcId)
        {
            if (!string.IsNullOrEmpty(npcId) && _activeNPCControllers.TryGetValue(npcId, out var ctrl))
            {
                var patrol = ctrl.GetComponent<NPCPatrolController>();
                if (patrol != null)
                    patrol.Pause();
            }
        }

        private void ResumeNPCPatrol(string npcId)
        {
            if (!string.IsNullOrEmpty(npcId) && _activeNPCControllers.TryGetValue(npcId, out var ctrl))
            {
                var patrol = ctrl.GetComponent<NPCPatrolController>();
                if (patrol != null)
                    patrol.Resume();
            }
        }

        private Dictionary<string, NPCDialogueController> _activeNPCControllers = new Dictionary<string, NPCDialogueController>();

        public void RegisterNPCController(NPCDialogueController controller)
        {
            if (!string.IsNullOrEmpty(controller.npcId))
                _activeNPCControllers[controller.npcId] = controller;
        }

        public void UnregisterNPCController(string npcId)
        {
            _activeNPCControllers.Remove(npcId);
        }

        public GlobalSettings GetGlobalSettings()
        {
            return _config?.globalSettings;
        }

        public bool IsDialogueTriggered(string npcId, string dialogueId)
        {
            return _triggeredDialogues.TryGetValue(npcId, out var triggered) &&
                   triggered.Contains(dialogueId);
        }

        public List<DialogueDefinition> GetDialoguesByTriggerForAllNPCs(TriggerType triggerType, int triggerAreaId = 0)
        {
            var result = new List<DialogueDefinition>();
            foreach (var npcId in _npcDialogues.Keys)
            {
                var dialogues = GetDialoguesByTrigger(npcId, triggerType, triggerAreaId);
                result.AddRange(dialogues);
            }
            return result;
        }

        public NPCDefinition GetNPCDefinitionForDialogue(string dialogueId)
        {
            foreach (var kvp in _npcDialogues)
            {
                if (kvp.Value.Any(d => d.dialogueId == dialogueId))
                    return GetNPCDefinition(kvp.Key);
            }
            return null;
        }
    }
}
