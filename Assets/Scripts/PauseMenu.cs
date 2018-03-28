using UnityEngine;
using UnityEngine.SceneManagement;

//INGAME MENU
public class PauseMenu : MonoBehaviour {
    public GameObject pauseMenuUI;
    public GameObject controls;
    
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
        pauseMenuUI.SetActive(false);
        controls.SetActive(true);
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        controls.SetActive(false);
        GameIsPaused = true;
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