using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

//MAINMENU

public class MainMenu : MonoBehaviour {
    public List<Button> buttons;
    public GameObject mainmenuPanel;
    public GameObject optionmenuPanel;

    static string game = "smashminigame";

    public AudioMixer mixer;
    public Slider slider;
    float volume;
    float mixerVolume;

    private void Update()
    {
        //mixer.GetFloat("volume", mixerVolume);
    }

    public void SetVolume(float v)
    {
        mixer.SetFloat("volume", v);
        volume = v;
    }

    private void Start()
    {
        volume = PlayerPrefs.GetFloat("volume");
        mixer.SetFloat("volume", volume);
        slider.value = volume;
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
            PlayerPrefs.SetFloat("volume", volume);
            SceneManager.LoadScene(game);
        }
        else if (i == 1)
        {
            optionmenuPanel.SetActive(true);
            mainmenuPanel.SetActive(false);
        }
        else if(i == 2)
        {
            Application.Quit();
        }
        else if(i == 3)
        {
            mainmenuPanel.SetActive(true);
            optionmenuPanel.SetActive(false);
        }
        else if(i == 7)
        {
            Debug.Log("mute sound add this ");
        }
    }

    public void OnMouseExit(int i)
    {
        Debug.Log("moveback");
        buttons[i].transform.Translate(-20.0f, 0f, 0f, Space.World);
    }
}