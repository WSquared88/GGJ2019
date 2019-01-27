using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour {

    public AudioManager Audio;

    void Start()
    {
        Audio.SourceA.clip = Audio.MainMenuTheme;
        Audio.SourceB.clip = Audio.MainTheme;
        Audio.SourceA.Play();
    }
}
