using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Complete : MonoBehaviour {
    public GameObject CompletePanel;
    public GameObject player1;
    public GameObject player2;
    
    Timer timer;
    Text endTimeText;

    public bool isComplete;
    
	void Start () {
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
        yield return new WaitForSeconds(5f);
        player1.GetComponent<Controller2D>().targetCount_ = 0;
        SceneManager.LoadScene("titlescreen");

    }
}