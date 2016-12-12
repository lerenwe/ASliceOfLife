using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    [SerializeField]
    AudioClip IntroMusic;
    [SerializeField]
    AudioClip WakeUpMusic;
    [SerializeField]
    AudioClip DreamMusic;

    AudioSource audioSource;

    public static bool Intro = false;
    public static bool WakeUp = false;
    public static bool Dream = false;

	// Use this for initialization
	void Start ()
    {
        audioSource = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Intro)
        {
            audioSource.clip = IntroMusic;
            audioSource.Play();
            Intro = false;
        }
	}
}
