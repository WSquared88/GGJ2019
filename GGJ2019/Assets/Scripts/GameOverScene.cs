using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScene : MonoBehaviour
{
    public AudioManager Audio;
    public float ScoreToWin = 10000;
    void Start()
    {
        if (ScoreManager.currScore < ScoreToWin)
        {
            Audio.SourceA.clip = Audio.LoseTheme;
        }
        else
        {
            Audio.SourceA.clip = Audio.WinTheme;
        }
        Audio.SourceA.Play();
    }
}
