using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class EndingManager : MonoBehaviour
{
    public GameObject endingsPanel;
    public TMP_Text endingText;
    private Image panelImage;
    void Start()
    {
        endingsPanel.SetActive(false);
        panelImage = endingsPanel.GetComponent<Image>();// hide at start
    }

    public void ShowEnding(int totalBadPoints)
    {
        endingsPanel.SetActive(true);

        if (totalBadPoints <= 6)
        {
            panelImage.color = new Color32(234, 255, 182, 255);
            endingText.text = "GOOD ENDING — YOU GOT AWAY\r\nYou trusted your instincts and made safe choices.\r\nThe stranger lost their control over you… and you found your way out of their world.\r\nYou’re safe — and stronger than before.\r\nRemember: the right choice can protect you in real life too.";
        }
        else
        {
            panelImage.color = new Color32(255, 150, 150, 255);
            endingText.text = "BAD ENDING — CAUGHT\r\nYou ignored the warning signs.\r\nYou trusted too quickly.\r\nAnd the stranger finally gained the control they wanted.\r\nNot all worlds let you escape.\r\nBe careful who you trust online.";
        }
        Time.timeScale = 0f;
    }
}
