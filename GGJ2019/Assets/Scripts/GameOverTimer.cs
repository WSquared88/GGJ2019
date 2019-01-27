using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverTimer : MonoBehaviour
{
    [SerializeField]
    float MaxTimeUntilGameOverInSeconds = 60.0f;
    float CurrentTimeUntilGameOverInSeconds;
    [SerializeField]
    Image TimerImageComponent;
    [SerializeField]
    Text TimerTextComponent;
    [SerializeField]
    string TimerStringFormat;

	// Use this for initialization
	void Start ()
    {
        CurrentTimeUntilGameOverInSeconds = MaxTimeUntilGameOverInSeconds;
        Debug.Assert(MaxTimeUntilGameOverInSeconds > 0.0f + float.Epsilon, "The gameover timer was set to less than zero when it was initialized!");
        Debug.Assert(TimerImageComponent, "The GameOverTimer doesn't have the TimerImageComponent set! We need this so we can make it animate!");

        if (!TimerTextComponent)
        {
            TimerTextComponent = GetComponentInChildren<Text>();
        }

        Debug.Assert(TimerTextComponent, "There isn't a TimerTextComponent attached to one of the children of the GameOverTimer!");
	}
	
	// Update is called once per frame
	void Update ()
    {
        CurrentTimeUntilGameOverInSeconds -= Time.deltaTime;
        TimerImageComponent.fillAmount = CurrentTimeUntilGameOverInSeconds / MaxTimeUntilGameOverInSeconds;
        TimerTextComponent.text = string.Format(TimerStringFormat, CurrentTimeUntilGameOverInSeconds);

        if(CurrentTimeUntilGameOverInSeconds <= 0.0f)
        {
            SceneManager.LoadScene("GameOverScene");
        }
	}
}
