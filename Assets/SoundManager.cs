using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class SoundManager : MonoBehaviour {

    [SerializeField]
    AudioClip IntroMusic;
    [SerializeField]
    AudioClip WakeUpMusic;
    [SerializeField]
    AudioClip DreamMusic;

    AudioSource mainAudioSource;
    AudioSource fadeInSource;
    AudioSource ambientSource;

    [SerializeField]
    float CrossFadeSpeed = 50f;

    [SerializeField]
    AudioMixer mixer;

    public static bool Intro = false;
    public static bool WakeUp = false;
    public static bool Dream = false;
    public static bool OutsideAmbiance = false;

    bool CrossFade = false;

    float MainMusicVol
    {
        get
        {
            float mainMusicVol;
            mixer.GetFloat("MainMusicVol", out mainMusicVol);
            return mainMusicVol;
        }
        set
        {
            mixer.SetFloat("MainMusicVol", value);
        }
    }

    float FadeInMusicVol
    {
        get
        {
            float fadeInMusicVol;
            mixer.GetFloat("FadeInMusicVol", out fadeInMusicVol);
            return fadeInMusicVol;
        }
        set
        {
            mixer.SetFloat("FadeInMusicVol", value);
        }
    }

    float AmbientVol
    {
        get
        {
            float ambientVol;
            mixer.GetFloat("AmbientVol", out ambientVol);
            return ambientVol;
        }
        set
        {
            mixer.SetFloat("AmbientVol", value);
        }
    }

    // Use this for initialization
    void Start ()
    {
        mainAudioSource = this.GetComponent<AudioSource>();
        fadeInSource = this.gameObject.transform.GetChild(0).GetComponent<AudioSource>();
        ambientSource= this.gameObject.transform.GetChild(1).GetComponent<AudioSource>();
        MainMusicVol = -80f;
        FadeInMusicVol = -80f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (OutsideAmbiance) //Clean this shit, it's very basic rn
        {
            AmbientVol = Mathf.MoveTowards(AmbientVol, 0f, CrossFadeSpeed * Time.deltaTime);
        }
        else
        {
            AmbientVol = Mathf.MoveTowards(AmbientVol, -80f, CrossFadeSpeed * Time.deltaTime);
        }

	    if (Intro)
        {
            Debug.Log("Intro Music Fading in...");
            mainAudioSource.clip = IntroMusic;

            if (MainMusicVol <= -80f)
                mainAudioSource.Play();

            MainMusicVol = Mathf.MoveTowards(MainMusicVol, 0, 50f * Time.deltaTime);

            if (MainMusicVol == 0)
            {
                Debug.Log("Intro Music Now Playing...");
                Intro = false;
            }
        }

        if (!Intro) //Make sure we finished to fade in the intro music before doing anything else.
        {
            if (WakeUp && !CrossFade)//Let's make sure another crossFade is not in progress before changing the clip of the Fade In Source
            {
                fadeInSource.clip = WakeUpMusic;
                CrossFade = true;
                WakeUp = false;
            }

            if (Dream && !CrossFade)
            {
                fadeInSource.clip = DreamMusic;
                CrossFade = true;
                Dream = false;
            }
        }

        if(CrossFade)
            CrossFading();
	}

    void CrossFading ()
    {
        if (FadeInMusicVol <= -80f && !fadeInSource.isPlaying)
            fadeInSource.Play();

        MainMusicVol = Mathf.MoveTowards(MainMusicVol, -80f, CrossFadeSpeed * Time.deltaTime);
        FadeInMusicVol = Mathf.MoveTowards(FadeInMusicVol, 0f , CrossFadeSpeed * Time.deltaTime);

        if (MainMusicVol == -80f && FadeInMusicVol == 0f)
        {
            Debug.Log("Finished crossfade");
            mainAudioSource.clip = fadeInSource.clip;

            int resumeTime = fadeInSource.timeSamples;
            mainAudioSource.timeSamples = resumeTime;

            fadeInSource.Stop();
            mainAudioSource.Play();
            MainMusicVol = 0f;
            FadeInMusicVol = -80f;
            CrossFade = false;
        }
    }
}
