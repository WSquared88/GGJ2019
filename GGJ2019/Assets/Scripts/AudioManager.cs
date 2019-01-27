using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioClip MainMenuTheme;
    public AudioClip MainTheme;
    public AudioClip WinTheme;
    public AudioClip LoseTheme;
    public float FadeDuration = 2;

    public AudioSource SourceA, SourceB;


    AudioSource Primary, Secondary;
    bool IsCrossfading = false;
    float CrossFadeTime = 0;

    
	// Use this for initialization
	void Awake ()
    {
        Primary = SourceA;
        Secondary = SourceB;
	}

    public void CrossFade()
    {
        IsCrossfading = true;
        SourceB.Play();
        CrossFadeTime = Time.time;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (IsCrossfading)
        {
            // Primary goes down, secondary goes up
            float t = Time.time - CrossFadeTime;
            t /= FadeDuration;
            SourceA.volume = Mathf.Lerp(1, 0, t);
            SourceB.volume = Mathf.Lerp(0, 1, t);
            if (t >= 1)
            {
                IsCrossfading = false;
            }
        }
	}
}
