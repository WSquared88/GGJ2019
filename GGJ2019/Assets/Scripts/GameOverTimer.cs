using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Text))]
public class GameOverTimer : MonoBehaviour
{
    [SerializeField]
    float TimeUntilGameOverInSeconds = 60.0f;
    Text TimerTextComponent;
    [SerializeField]
    string TimerCountdownString;

	// Use this for initialization
	void Start ()
    {
        Debug.Assert(TimeUntilGameOverInSeconds > 0.0f + float.Epsilon, "The gameover timer was set to less than zero when it was initialized!");
        TimerTextComponent = GetComponent<Text>();
        Debug.Assert(TimerTextComponent != null, "The timer was unable to find the text component on it!");
        //Debug.Assert(TimerCountdownString.Length > 0 && TimerCountdownString.Contains("{0}"), "The timer countdown string doesn't have a {0} in the string. We won't be able to display the time left!");
	}
	
	// Update is called once per frame
	void Update ()
    {
        TimeUntilGameOverInSeconds -= Time.deltaTime;
        TimerTextComponent.text = string.Format(TimerCountdownString, TimeUntilGameOverInSeconds);

        if(TimeUntilGameOverInSeconds <= 0.0f)
        {
            SceneManager.LoadScene("GameOverScene");
        }
	}
}
