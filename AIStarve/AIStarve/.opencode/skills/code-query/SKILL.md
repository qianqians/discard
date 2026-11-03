---
name: code-query
description: Query code functionality, related classes, and associated code using knowledge graph. Use code-graph-rag tool when understanding class, function, or code block functionality.
license: MIT
compatibility: opencode
metadata:
    domain: code-analysis
    tool: code-graph-rag
---

## What I do

- Use `code-graph-rag_query_code_graph` tool to query code functionality, related classes, and associated code
- Use `code-graph-rag_get_node_details` to get detailed node metadata and relationships
- If query results return specific code locations (qualified_name), use standard methods to find concrete implementations
- Return code or class functionality descriptions

## When to use me

Use this skill when:
- User asks about the functionality of a class or function
- Need to find related classes or associated code
- Need to understand code structure

## Usage Guidelines

- Prioritize knowledge graph queries
- Only use the `code-graph-rag` MCP tool `query_code_graph` and `get_node_details`
- Do not call other MCP tools
- **必须先查 MCP，仅当 MCP 不可用时回退到 read/grep**

## Qualified Name Construction

限定名的构造规则（必须遵守）：

```
AIStarve.AIStarve.Assets.Scripts.<文件夹路径>.<类名>
```

示例：
| 类名 | 限定名 |
|------|--------|
| GameSceneManager | `AIStarve.AIStarve.Assets.Scripts.Scene.SceneManager.GameSceneManager` |
| CharacterController | `AIStarve.AIStarve.Assets.Scripts.CharacterController.CharacterController` |
| SceneTriggerExample | `AIStarve.AIStarve.Assets.Scripts.SceneTriggerExample.SceneTriggerExample` |
| NPCDialogueController | `AIStarve.AIStarve.Assets.Scripts.NPC.NPCDialogueController.NPCDialogueController` |
| NPCDialogueManager | `AIStarve.AIStarve.Assets.Scripts.NPC.NPCDialogueManager.NPCDialogueManager` |
| TriggerArea2D | `AIStarve.AIStarve.Assets.Scripts.TriggerArea2D.TriggerArea2D` |
| RemoteCharacterController | `AIStarve.AIStarve.Assets.Scripts.Scene.RemoteCharacterController.RemoteCharacterController` |

规则：
1. 前缀固定：`AIStarve.AIStarve.Assets.Scripts.`
2. 中间路径：对应 `Assets/Scripts/` 下的子目录，用 `.` 替代 `/`
3. 最后重复类名：文件名和类名相同则只出现一次，不同则 `文件名.类名`

## Tool Usage Specification

1. **Query code functionality**:
```javascript
code-graph-rag_query_code_graph({
  natural_language_query: "User's question",
  limit: 10,
  enable_llm_descriptions: true
})
```

2. **Get node details by qualified name**:
```javascript
code-graph-rag_get_node_details({
  qualified_name: "AIStarve.AIStarve.Assets.Scripts.xxx.ClassName"
})
```

## Example Usage

Use this skill when the LLM needs to query code-related functionality:
- "What does this function do?"
- "Find classes related to UserService"
- "Explain the functionality of this code block"
- "What related classes does this class have?"
