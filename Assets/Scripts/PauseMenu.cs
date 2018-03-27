using UnityEngine;
using UnityEngine.SceneManagement;

//INGAME MENU
public class PauseMenu : MonoBehaviour {
    static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject controls;

    private Vector3 posHold;

    static string mainmenu = "titlescreen";

    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Home"))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
	}

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        /*controls.GetComponent<PlayerController>().enabled = true;
        controls.transform.position = posHold;*/
        controls.SetActive(true);
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        /*controls.GetComponent<PlayerController>().enabled = false;
        posHold = controls.transform.position;*/
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