using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Audio;

//public class Volume : MonoBehaviour {
//    public AudioMixer mixer;
//    public Slider slider;
//    float _volume;

//	// Update is called once per frame
//	public void SetVolume (float volume) {
//        mixer.SetFloat("volume", volume);
//	}

//    private void Awake()
//    {
//        _volume = PlayerPrefs.GetFloat("volume");
//        mixer.SetFloat("volume", _volume);
//        gameObject.GetComponent<Slider>().value = _volume;
//    }

//    private void OnDestroy()
//    {
//        PlayerPrefs.SetFloat("volume", _volume);
//    }
//}