using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayScript : MonoBehaviour {
    List<Target> targetz = new List<Target>();


    void Start () {
        targetz.AddRange(FindObjectsOfType<Target>());
        print(FindObjectsOfType<Target>());
	}
	
	void Update () {
		
	}
}