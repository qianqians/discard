using UnityEngine;
using SceneManagement;
using System.Collections.Generic;
using System.Linq;

namespace NPCDialogue
{
    public class NPCAreaTriggerBridge : MonoBehaviour
    {
        private NPCDialogueManager _manager;
        private TriggerArea2D _triggerArea;
        private int _triggerAreaId = -1;

        private void Start()
        {
            _manager = NPCDialogueManager.Instance;
            _triggerArea = GetComponent<TriggerArea2D>();
            if (_triggerArea != null)
            {
                _triggerArea.OnObjectEntered += OnObjectEnteredArea;
            }

            var textAsset = Resources.Load<TextAsset>("TriggerAreaMapping");
            if (textAsset != null)
            {
                var mapping = JsonUtility.FromJson<TriggerAreaMappingData>(textAsset.text);
                if (mapping?.triggerAreas != null)
                {
                    var entry = mapping.triggerAreas.FirstOrDefault(a => a.gameObjectName == gameObject.name);
                    if (entry != null)
                        _triggerAreaId = entry.id;
                }
            }
        }

        private void OnDestroy()
        {
            if (_triggerArea != null)
            {
                _triggerArea.OnObjectEntered -= OnObjectEnteredArea;
            }
        }

        private void OnObjectEnteredArea(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (_manager == null || _triggerAreaId < 0) return;

            var dialogues = _manager.GetDialoguesByTriggerForAllNPCs(TriggerType.enter_area, _triggerAreaId);
            foreach (var dialogue in dialogues)
            {
                var npcDef = _manager.GetNPCDefinitionForDialogue(dialogue.dialogueId);
                if (npcDef != null)
                {
                    _manager.TriggerDialogue(npcDef.npcId, dialogue.dialogueId);
                }
            }
        }

        [System.Serializable]
        private class TriggerAreaMappingData
        {
            public List<TriggerAreaEntry> triggerAreas;
        }

        [System.Serializable]
        private class TriggerAreaEntry
        {
            public int id;
            public string gameObjectName;
        }
    }
}
