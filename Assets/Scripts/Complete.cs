using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Complete : MonoBehaviour {
    public GameObject CompletePanel;
    public GameObject player1;
    public GameObject player2;
    private static int tc;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {
        tc = PlayerController.targetCount;

        if (tc >= 10)
        {
            CompletePanel.SetActive(true);
            player1.SetActive(false);
            player2.SetActive(false);
        }
	}
}