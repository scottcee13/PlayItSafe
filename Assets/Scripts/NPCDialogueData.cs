using UnityEngine;

[System.Serializable]
public class DialogueOption
{
    public string optionText;
    public int badPoints;
}

[System.Serializable]
public class DialogueLine
{
    [TextArea(2, 5)]
    public string dialogueText;

    public DialogueOption option1;
    public DialogueOption option2;
}

public class NPCDialogueData : MonoBehaviour
{
    public DialogueLine[] dialogueLines;
}
