using UnityEngine;
using UnityEngine.SceneManagement;

//INGAME MENU
public class PauseMenu : MonoBehaviour {
    public GameObject pauseMenuUI;
    public GameObject controlsP1;
    public GameObject controlsP2;
    public AudioSource eaux;
    public AudioClip pauseClip;
    public AudioClip menuBack;
    public AudioClip menuOk;
    
    static bool GameIsPaused = false;
    string mainmenu = "titlescreen";

    void Update () {
        if (Input.GetButtonDown("Home"))
        {
            if (GameIsPaused)
            {
                Time.timeScale = 1f;
                Resume();
            }
            else
            {
                Time.timeScale = 0f;
                Pause();
            }
        }
	}

    public void Resume()
    {
        eaux.clip = menuOk;
        eaux.Play();
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        controlsP1.SetActive(true);
        controlsP2.SetActive(true);
    }

    public void Pause()
    {
        eaux.clip = pauseClip;
        eaux.Play();
        GameIsPaused = true;
        pauseMenuUI.SetActive(true);
        controlsP1.SetActive(false);
        controlsP2.SetActive(false);
    }

    public void SwitchToMenu()
    {
        eaux.clip = menuBack;
        eaux.Play();
        Debug.Log("menu");
        SceneManager.LoadScene(mainmenu);
    }

    public void QuitGame()
    {
        eaux.clip = menuBack;
        eaux.Play();
        Debug.Log("QuitW");
        Application.Quit();
    }
}