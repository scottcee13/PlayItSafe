using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private NPCDialogueData data;
    private DialogueManager manager;
    private bool triggered = false;
    public bool isFinalNPC = false;
    void Awake()
    {
        data = GetComponent<NPCDialogueData>();
        manager = FindObjectOfType<DialogueManager>();

        if (data == null)
            Debug.LogError("NPCDialogueData missing on " + gameObject.name);
        if (manager == null)
            Debug.LogError("DialogueManager not found in scene");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            manager.StartDialogue(data.dialogueLines, this);
        }
    }
}
