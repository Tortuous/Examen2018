﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {
    public Text timer;
    public Text textCount;
    public string timerFormatted;
    public List<GameObject> controls = new List<GameObject>();

    float elapsedTime;
    float timeDown = 3f;
    string textVisual;

	void Start () {
        foreach (GameObject player in controls)
        {
            player.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {

        StartCountDown();
    }

    private void StartCountDown()
    {
        if (timeDown > 0)
        {
            if (timeDown > 2)
            {
                textVisual = "READY?";
            }
            else if (timeDown < 2 && timeDown > 0.5f)
            {
                textVisual = "SET!";
            }
            else
            {
                textVisual = "GO!";
            }
            textCount.text = textVisual;
            timeDown -= Time.deltaTime;
            System.TimeSpan t = System.TimeSpan.FromSeconds(timeDown);
            timerFormatted = string.Format("{0:D1}.{1:D2}", t.Seconds, t.Milliseconds);
            timer.text = timerFormatted;
        }
        else
        {
            timer.text = "0.00";
            foreach (GameObject player in controls)
            {
                player.SetActive(true);
            }
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }
}