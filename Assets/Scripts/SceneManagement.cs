using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagement : MonoBehaviour {
    int counter = 0;

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Home") && counter == 0)
        {
            Time.timeScale = 0;
            counter++;
        }
        if(Input.GetButtonDown("Home")&& counter > 0)
        {
            Time.timeScale = 1;
            counter = 0;
        }
	}
}