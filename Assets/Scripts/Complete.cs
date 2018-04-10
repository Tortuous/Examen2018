using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Complete : MonoBehaviour {
    public GameObject CompletePanel;
    public GameObject player1;
    public GameObject player2;
    public AudioSource eaux;
    public AudioClip claps;

    Timer timer;
    Text endTimeText;
    int audioPlayAmount = 1;

    public bool isComplete;
    
	void Start () {
        audioPlayAmount = 1;
        timer = GetComponent<Timer>();
        endTimeText = CompletePanel.GetComponentInChildren<Text>();
    }
	
	void Update ()
    {
        if (player1.GetComponent<Controller2D>().targetCount_ >= 10)
        {
            isComplete = true;
            CompletePanel.SetActive(true);
            player1.SetActive(false);
            player2.SetActive(false);
            StartCoroutine(BackToMain());
            endTimeText.text = "Time: " + timer.timerFormatted;
        }
	}

    IEnumerator BackToMain()
    {
        if(audioPlayAmount == 1)
        {
            eaux.clip = claps;
            eaux.Play();
            audioPlayAmount--;
        }
        yield return new WaitForSeconds(5f);
        player1.GetComponent<Controller2D>().targetCount_ = 0;
        SceneManager.LoadScene("titlescreen");

    }
}