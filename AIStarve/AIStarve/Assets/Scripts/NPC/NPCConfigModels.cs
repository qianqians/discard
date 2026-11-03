using System.Collections.Generic;
using UnityEngine;

namespace NPCDialogue
{
    [System.Serializable]
    public class NPCDialogueConfig
    {
        public List<NPCDefinition> npcs = new List<NPCDefinition>();
        public GlobalSettings globalSettings = new GlobalSettings();
    }

    [System.Serializable]
    public class NPCDefinition
    {
        public string npcId;
        public string npcName;
        public string scriptName;
        public string prefabPath;
        public Vector2 spawnPosition;
        public List<DialogueDefinition> dialogues = new List<DialogueDefinition>();
        public NPCPatrolConfig patrol;
    }

    [System.Serializable]
    public class NPCPatrolConfig
    {
        public float speed = 2f;
        public bool loop = true;
        public float waitSeconds = 3f;
        public List<int> triggerAreaIds = new List<int>();
    }

    [System.Serializable]
    public class DialogueDefinition
    {
        public string dialogueId;
        public string triggerType;
        public int triggerAreaId;
        public bool firstTimeOnly;
        public List<string> conditionTags = new List<string>();
        public List<string> excludeTags = new List<string>();
        public List<ConversationLine> conversations = new List<ConversationLine>();
        public List<DialogueAction> onStartActions = new List<DialogueAction>();
        public List<DialogueAction> onEndActions = new List<DialogueAction>();
        public List<DialogueChoice> choices = new List<DialogueChoice>();
    }

    [System.Serializable]
    public class ConversationLine
    {
        public string speaker;
        public string text;
        public string type = "speak";
        public string animation;
        public List<DialogueAction> onSpeakActions = new List<DialogueAction>();
    }

    [System.Serializable]
    public class DialogueAction
    {
        public string action;
        public string message;
        public string targetType;
        public Vector2 target;
        public int triggerAreaId;
        public string npcId;
    }

    [System.Serializable]
    public class DialogueChoice
    {
        public string choiceId;
        public string text;
        public string nextDialogueId;
        public List<string> addTags = new List<string>();
        public List<DialogueAction> onSelectActions = new List<DialogueAction>();
        public int affinityDelta;
        public string affinityCondition;
    }

    [System.Serializable]
    public class GlobalSettings
    {
        public string defaultPrefabPath = "Perfabs/Characters/OtherCharacter";
        public float moveSpeed = 3.0f;
    }

    public enum TriggerType
    {
        interact,
        enter_area,
        choice_result
    }

    public enum DialogueActionType
    {
        log,
        move
    }

    public enum TargetType
    {
        point,
        triggerArea
    }
}
