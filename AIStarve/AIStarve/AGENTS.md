# AIStarve Project - Agent Guidelines

This document provides guidelines for AI agents working with the AIStarve Unity project.

## Project Overview

- **Engine**: Unity 2022.3.61f1
- **Platform**: Windows Standalone (64-bit)
- **Language**: C# (.NET Framework 4.7.1, LangVersion 9.0)
- **Type**: 2D multiplayer farming/survival game
- **Architecture**: Client-server with WebSocket communication

## Build Commands

### Unity Build (Editor)
1. Open Unity Editor and load the project.
2. **File → Build Settings** → Select target platform (Windows).
3. Click **Build** (output to `Build/`).

### Asset Bundles
- Use the **Tools/Build AssetBundles** menu item in Unity Editor.
- This builds all prefabs in `Assets/Prefabs` into bundles in `Assets/StreamingAssets/AssetBundles/`.
- The builder supports incremental updates and generates version/hash files.

### Command Line Build (Batch Mode)
```bash
Unity.exe -projectPath "D:\Program Files\Fork\AIStarve\AIStarve" -batchmode -quit -executeMethod AssetBundleBuilder.BuildAllAssetBundles
```

## Test Commands

### Unity Test Runner (Editor)
1. **Window → General → Test Runner**.
2. Switch to **PlayMode** or **EditMode**.
3. Run all tests or select specific assemblies.

### Command Line Test Execution
```bash
Unity.exe -projectPath "D:\Program Files\Fork\AIStarve\AIStarve" -runTests -batchmode -testPlatform playmode -quit
```

### Test Assemblies
- The project includes `com.unity.test-framework` (v1.1.33).
- No dedicated test assembly defined yet; manual test scripts are in `Assets/Scripts/Test/`.
- Use `#if IS_TEST` preprocessor directives for test-only code.

## Linting & Code Analysis

### Unity's Built‑in Analysis
- Enable **Edit → Preferences → Analysis → .NET Compiler Errors**.
- The `.csproj` files suppress warnings `0169` (unused variable) and `USG0001`.
- Target framework: `.NET Framework 4.7.1`.

### Roslyn Analyzers
No custom Roslyn analyzers are configured. Rely on Unity's compiler warnings.

## Code Style Guidelines

### Indentation & Braces
- **Indent size**: 2 spaces (no tabs).
- **Braces**: Allman style (opening brace on new line).
```csharp
private void Example()
{
  if (condition)
  {
    // ...
  }
}
```

### Regions
Use `#region` / `#endregion` to group related members in large classes. Observed regions:
- `#region Constants`
- `#region Inspector` (serialized fields)
- `#region Fields` (private fields)
- `#region MonoBehaviour` (Awake, Start, Update, etc.)
- `#region Properties`

### Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Class, Struct, Enum | PascalCase | `GameSceneManager` |
| Interface | PascalCase with `I` prefix | `IWebSocket` |
| Public methods, properties | PascalCase | `RegisterEvent`, `SceneDB` |
| Private fields | camelCase with `_` prefix (preferred) | `_animator`, `_eventQueue` |
| Local variables | camelCase | `cellPosition`, `prefabPath` |
| Constants | UPPER_SNAKE_CASE | `ANIM_WALKING` |
| Static readonly fields | PascalCase (or UPPER_SNAKE_CASE for hashes) | `_instance`, `ANIM_DIRECTION` |
| Serialized fields | private with `[SerializeField]` attribute, camelCase (no underscore) | `speed`, `runtimeAnimatorController` |

**Note**: Some existing private fields omit the underscore (e.g., `directionCur`, `fKeyPerfabId`). Follow the underscore convention for new code.

### Using Directives
Group and order imports as follows:
1. `System.*`
2. `System.Collections.*`
3. `System.Linq`
4. `UnityEngine.*`
5. `UnityEngine.*` submodules
6. Third‑party namespaces (e.g., `Abelkhan`)
7. Project‑specific namespaces (e.g., `SceneManagement`)

Example:
```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Abelkhan;
using System.Linq;
```

### Type Inference
Use `var` when the type is obvious from the right‑hand side (e.g., `var position = new Vector3(1, 2, 3);`). For primitive types (`int`, `float`, `string`), explicit typing is acceptable.

### Logging
- Use `Debug.Log()` for general information.
- Use `Debug.LogWarning()` for non‑critical issues.
- Use `Debug.LogError()` for errors that should be fixed.
- Include relevant context (method name, variable values) in log messages.
- Remove or comment out excessive debug logs before committing.

### XML Documentation
Public APIs (classes, methods, properties) should have XML documentation:
```csharp
/// <summary>
/// Registers an event handler for the specified scene event.
/// </summary>
/// <param name="eventType">The type of scene event.</param>
/// <param name="handler">The action to invoke when the event occurs.</param>
public void RegisterEvent(SceneEvent eventType, Action<SceneEventArgs> handler)
```

### Preprocessor Directives
- `#if IS_TEST` marks code that should only run in test builds.
- `#if UNITY_EDITOR` for editor‑only code (e.g., custom inspectors).
- Always provide `#else` or `#endif` clauses.

### Serialized Fields & Inspector Attributes
- Private serialized fields should be marked with `[SerializeField]`.
- Use Inspector attributes (`[Header]`, `[Min]`, `[Tooltip]`) to improve usability.
```csharp
[Header("角色动画控制器")]
[SerializeField]
private RuntimeAnimatorController runtimeAnimatorController;
```

### Error Handling
- Use `Debug.LogError()` for unrecoverable errors; consider throwing exceptions in non‑performance‑critical code.
- Validate arguments in public methods (null checks, range checks).
- Use `try`‑`catch` only for expected exceptions (e.g., network timeouts).

### MonoBehaviour Lifecycle
- Place `Awake()` for initialization that doesn’t depend on other objects.
- Use `Start()` for initialization that depends on other objects being `Awake()`.
- Use `FixedUpdate()` for physics‑related updates.
- Use `Update()` for frame‑based logic.
- Use `LateUpdate()` for camera‑following or post‑processing.

## Project‑Specific Patterns

### Singleton Pattern
Many manager classes use a static `Instance` property with a private `_instance` field. Ensure thread‑safe initialization (if needed) and guard against duplicate instances in `Awake()`.

### Event System
The game uses a custom event system (`SceneEvent`, `UIEventType`). Register handlers with `RegisterEvent` and trigger events with `TriggerEvent`. Events can be queued for deferred processing.

### Network Communication
- Network calls are made through `netScene`, `netLogin`, etc., in the `netDriver` namespace.
- Server‑side SDK code is in `Assets/Scripts/serverSDK/`. Do not modify generated RPC files.

### Tilemap Interaction
- Use `Tilemap.WorldToCell()` and `Tilemap.GetCellCenterWorld()` to align objects to grid.
- `FarmingTilemap` holds cultivated land; `Ground Tilemap` is for walking.
- RuleTile resources are loaded via `SceneDataBase.GetPrefabPath(id)`.

## File & Directory Conventions
- Scripts are organized under `Assets/Scripts/` by feature (Scene, UI, netDriver, serverSDK, etc.).
- Editor scripts reside in `Assets/Editor/` or `Assets/Scripts/Editor/`.
- Prefabs are stored in `Assets/Prefabs/`.
- Resources (JSON, icons) are placed in `Assets/Resources/`.
- Asset Bundles are output to `Assets/StreamingAssets/AssetBundles/`.

## Version Control
- The repository uses Git. Follow the existing `.gitignore` (Unity‑specific excludes).
- Commit messages should be descriptive and reference relevant issues if any.
- Do not commit `Library/`, `Temp/`, `Obj/`, `Build/`, or user‑specific files.

## Additional Notes
- The project contains Chinese comments and UI strings; maintain them as is.
- When adding new features, ensure they work both in standalone and WebGL builds (WebSocket support is already included).
- If you need to modify the `PrefabMapping.json` or `KeyBoardMapping.json`, update the corresponding loader in `SceneDataBase`.

---

## AI Agent Tool Guidelines

### Tool Priority
When working with this project, follow this tool priority order:

1. **Code Graph RAG (Preferred for code queries)**
   - Use for: finding classes, methods, references, callers, inheritance
   - Tools: `code-graph-rag_query_code_graph`, `code-graph-rag_get_code_snippet`, `code-graph-rag_get_node_details`

2. **Standard Tools (Fallback)**
   - Use for: config files, non-code files, when graph has no results
   - Tools: `grep`, `read`, `glob`

### Recommended Workflow
```
1. Query code structure → code-graph-rag_query_code_graph
2. Get specific snippet → code-graph-rag_get_code_snippet
3. Read full file (if needed) → read
4. Modify code → code-graph-rag_surgical_replace_code or edit
```

### Example Queries
- "What functions call UserService.create_user?" → `code-graph-rag_query_code_graph`
- "Show me the GameSceneManager class" → `code-graph-rag_get_code_snippet`
- "Find all references to SceneEvent" → `code-graph-rag_query_code_graph`

---

*This document is intended for AI agents assisting with development. Update it as the project evolves.*