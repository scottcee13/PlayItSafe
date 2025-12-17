using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseScene : MonoBehaviour
{
    public GameObject pauseLogo;
    public GameObject pauseMenu;
    public GameObject settings;
    public GameObject backgroundPause;
    public GameObject choicePanel;
    public GameObject ending;
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
    }

    public void GoSettings()
    {
        settings.SetActive(true);
    }
    public void ExitSettings()
    {
        settings.SetActive(false);
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        pauseLogo.SetActive(true);
        backgroundPause.SetActive(false);
        Time.timeScale = 1f;
    }
    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(1);
    }
    private void Update()
    {
        if (choicePanel.activeSelf == true)
        {
            pauseLogo.SetActive(false);
        }
        else
        {
            pauseLogo.SetActive(true);
        }
        if(ending.activeSelf == true)
        {
            pauseLogo.SetActive(false);
        }
    }
}
