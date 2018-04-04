using UnityEngine;
using UnityEngine.SceneManagement;

//INGAME MENU
public class PauseMenu : MonoBehaviour {
    public GameObject pauseMenuUI;
    public GameObject controlsP1;
    public GameObject controlsP2;
    
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
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        controlsP1.SetActive(true);
        controlsP2.SetActive(true);
    }

    public void Pause()
    {
        GameIsPaused = true;
        pauseMenuUI.SetActive(true);
        controlsP1.SetActive(false);
        controlsP2.SetActive(false);
    }

    public void SwitchToMenu()
    {
        Debug.Log("menu");
        SceneManager.LoadScene(mainmenu);
    }

    public void QuitGame()
    {
        Debug.Log("QuitW");
        Application.Quit();
    }
}