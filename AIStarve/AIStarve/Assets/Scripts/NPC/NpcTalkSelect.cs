using UnityEngine;

public class NpcTalkSelect : MonoBehaviour
{
    private NPCDialogue.NPCDialogueManager _manager;

    private void Start()
    {
        _manager = NPCDialogue.NPCDialogueManager.Instance;
    }

    private void Update()
    {
        if (_manager == null) return;

        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                _manager.TrySelectChoice(i);
            }
        }
    }
}
