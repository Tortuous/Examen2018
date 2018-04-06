using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    Complete complete;
    public Text timer;
    float elapsedTime;
    public string timerFormatted;

    private void Start()
    {
        complete = GetComponent<Complete>();
    }

    void Update () {
        if(!complete.isComplete)
        {
            CountUp();
        }
    }

    public void CountUp()
    {
        elapsedTime += Time.deltaTime;
        System.TimeSpan t = System.TimeSpan.FromSeconds(elapsedTime);
        timerFormatted = string.Format("{0:D}:{1:D2}.{2:D2}", t.Minutes, t.Seconds, t.Milliseconds);
        timer.text = timerFormatted;
    }
}