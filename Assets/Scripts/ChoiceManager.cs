using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public Text dialogueText;
    public Button choiceBtn1;
    public Button choiceBtn2;
    public Text btn1Text;
    public Text btn2Text;
    private DialogueTrigger currentNPC;

    [Header("Global Settings")]
    public int badEndingThreshold = 3;

    // internal
    private int totalBadPoints = 0;
    private DialogueLine[] currentLines;
    private int currentIndex = 0;

    public void StartDialogue(DialogueLine[] lines, DialogueTrigger npc)
    {
        currentLines = lines;
        currentIndex = 0;
        currentNPC = npc;
        ShowCurrentLine();
    }


    void ShowCurrentLine()
    {
        DialogueLine line = currentLines[currentIndex];

        dialoguePanel.SetActive(true);
        Time.timeScale = 0f;

        dialogueText.text = line.dialogueText;
        btn1Text.text = line.option1.optionText;
        btn2Text.text = line.option2.optionText;

        choiceBtn1.onClick.RemoveAllListeners();
        choiceBtn2.onClick.RemoveAllListeners();

        choiceBtn1.onClick.AddListener(() => Choose(line.option1.badPoints));
        choiceBtn2.onClick.AddListener(() => Choose(line.option2.badPoints));
    }

    void Choose(int badPoints)
    {
        totalBadPoints += badPoints;

        if (BadPointsTracker.Instance != null)
            BadPointsTracker.Instance.AddBadPoints(badPoints);
        else
            Debug.LogError("BadPointsTracker instance not found!");

        currentIndex++;

        if (currentIndex >= currentLines.Length)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        Time.timeScale = 1f;

        // Make NPC disappear after dialogue
        if (currentNPC != null)
            Destroy(currentNPC.gameObject);

        // Only trigger ending if this is the final NPC
        if (currentNPC != null && currentNPC.isFinalNPC)
        {
            FindObjectOfType<EndingManager>().ShowEnding(totalBadPoints);
        }
    }

}
