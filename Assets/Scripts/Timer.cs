using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    public Text timer;

    float elapsedTime;
    string timerFormatted;

	void Update () {
        CountUp();
	}

    public void CountUp()
    {
        elapsedTime += Time.deltaTime;
        System.TimeSpan t = System.TimeSpan.FromSeconds(elapsedTime);
        timerFormatted = string.Format("{0:D}:{1:D2}.{2:D2}", t.Minutes, t.Seconds, t.Milliseconds);
        timer.text = timerFormatted;
    }
}