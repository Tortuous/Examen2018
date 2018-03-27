using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

//MAINMENU

public class MainMenu : MonoBehaviour {
    public Vector3 position;
    public List<Button> buttons;

    static string game = "smashminigame";
    public void Awake()
    {
        position = transform.position;
    }

    public void OnMouseEnter(int i)
    {
        Debug.Log("moveleft");
        buttons[i].transform.Translate(20.0f, 0f, 0f, Space.World);
    }
    public void OnMouseClick(int i)
    {
        Debug.Log("Clicked");
        if (i == 0)
        {
            SceneManager.LoadScene(game);
        }
        else if (i == 1)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(2));
        }
        else if(i == 2)
        {
            Application.Quit();
        }
    }

    public void OnMouseExit(int i)
    {
        Debug.Log("moveback");
        buttons[i].transform.Translate(-20.0f, 0f, 0f, Space.World);
    }
}