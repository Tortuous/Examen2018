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
    public GameObject characterPanel;
    public AudioSource menuAudio;

    public AudioClip menuOk;
    public AudioClip menuBack;

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
        if (i == 0) // starts game
        {
            menuAudio.clip = menuOk;
            menuAudio.Play();
            PlayerPrefs.SetFloat("volume", volume);
            SceneManager.LoadScene(game);
        }
        else if (i == 1) // optionmenu
        {
            menuAudio.clip = menuOk;
            menuAudio.Play();
            optionmenuPanel.SetActive(true);
            mainmenuPanel.SetActive(false);
            characterPanel.SetActive(false);
        }
        else if(i == 2) // quit 
        {
            menuAudio.clip = menuBack;
            menuAudio.Play();
            Application.Quit();
        }
        else if(i == 3) //mainmenu
        {
            menuAudio.clip = menuBack;
            menuAudio.Play();
            mainmenuPanel.SetActive(true);
            optionmenuPanel.SetActive(false);
            characterPanel.SetActive(false);
        }
        else if(i == 4) //charactermenu
        {
            menuAudio.clip = menuOk;
            menuAudio.Play();
            mainmenuPanel.SetActive(false);
            characterPanel.SetActive(true);
            optionmenuPanel.SetActive(false);
        }
        else if(i == 7) //mutessound
        {
            menuAudio.clip = menuOk;
            menuAudio.Play();
            Debug.Log("mute sound add this ");
        }
    }

    public void OnMouseExit(int i)
    {
        Debug.Log("moveback");
        buttons[i].transform.Translate(-20.0f, 0f, 0f, Space.World);
    }
}