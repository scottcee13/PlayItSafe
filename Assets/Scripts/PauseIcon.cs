using UnityEngine;

public class PauseIcon : MonoBehaviour
{
    public GameObject pauseLogo;
    public GameObject pauseMenu;
    public GameObject backgroundPause;
    void Update()
    {
        if(pauseMenu.activeSelf == true)
        {
            pauseLogo.SetActive(false);
            backgroundPause.SetActive(true);
        }    
    }
}
