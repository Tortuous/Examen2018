using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Volume : MonoBehaviour {
    public AudioMixer mixer;

	// Update is called once per frame
	public void SetVolume (float volume) {
        mixer.SetFloat("volume", volume);
	}
}