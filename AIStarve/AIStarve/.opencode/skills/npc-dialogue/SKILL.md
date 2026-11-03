---
name: npc-dialogue
description: NPC Dialogue system with branching choices and affinity (好感度) system. Manages JSON-driven dialogue trees, choice branching, and persistent NPC affinity.
license: MIT
compatibility: opencode
metadata:
    domain: game-design
    tool: npc-dialogue
---

## What I do

- Drive NPC dialogue through `NPCDialogueManager` and `NPCDialogue.json`
- Provide branching choices (fixed 2-choice limit per dialogue step)
- Track NPC affinity (好感度) per NPC, range -10 to 10
- Persist affinity data via `Application.persistentDataPath/NPCAffinity.json`
- Filter choices by affinity conditions

## Core Architecture

### Data Flow
```
NPCDialogue.json (Resources) ──▶ NPCDialogueManager ──▶ ExecuteDialogue ──▶ choices
                                         │                            │
                                         ▼                            ▼
                              NPCAffinity.json (persistent)    NpcTalkSelect (1-9 key)
                                         ▲                            │
                                         └── SelectChoice ◀──────────┘
                                              │
                                              ├── apply affinityDelta
                                              ├── addTags
                                              └── trigger nextDialogue
```

### Key Files
| File | Purpose |
|------|---------|
| `Assets/Resources/NPC/NPCDialogue.json` | Default dialogue trees (read-only source) |
| `Application.persistentDataPath/NPCAffinity.json` | Per-Player affinity data (auto-created) |
| `Assets/Scripts/NPC/NPCConfigModels.cs` | C# data models for JSON deserialization |
| `Assets/Scripts/NPC/NPCDialogueManager.cs` | Singleton manager: load, trigger, select |
| `Assets/Scripts/NPC/NPCDialogueController.cs` | NPC-side component, listens for player interaction |
| `Assets/Scripts/CharacterController.cs` | Player F-key detection → NPCDialogueController.OnInteract |
| `Assets/Scripts/NPC/NpcTalkSelect.cs` | Player-side component, 1-9 key → TrySelectChoice |

## Dialogue Choice Rules

1. **Each dialogue shows exactly 2 choices** — but `affinityCondition` may filter one out, leaving fewer
2. Every choice must have an `affinityDelta` value (0 = no change)
3. Every choice must have a `nextDialogueId` (null = end of conversation)
4. `affinityCondition` format: supports `>=N`, `<=N`, `>N`, `<N`, `==N`. Empty = always shown

## JSON Schema

```json
{
  "choices": [
    {
      "choiceId": "c_xxx_yyy",
      "text": "显示的选项文本",
      "nextDialogueId": "d_next_dialogue",
      "addTags": ["tag_name"],
      "onSelectActions": [],
      "affinityDelta": 2,
      "affinityCondition": ">=0"
    }
  ]
}
```

## Affinity Persistence

Follows `KeyBoardMapping.json` pattern:
- **First load**: defaults from `Resources/NPC/NPCDialogue.json` (all affinity = 0)
- **Persistent**: `Application.persistentDataPath/NPCAffinity.json`
- **Save trigger**: auto-saved after every choice that modifies affinity
- **Range**: clamped to -10 to 10

## When to use me

Use this skill when:
- Modifying NPC dialogue data (branching, choices, conversations)
- Adding new NPCs or dialogues
- Debugging dialogue flow or affinity issues
- Understanding the NPC interaction pipeline
