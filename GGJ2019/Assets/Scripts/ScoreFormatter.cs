using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreFormatter : MonoBehaviour
{
    private Text TextToFormat;
    public bool UpdateEveryFrame = false;

	// Use this for initialization
	void Start ()
    {
        TextToFormat = GetComponent<Text>();
        TextToFormat.text = string.Format(TextToFormat.text, ScoreManager.currScore);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (UpdateEveryFrame)
        {
            TextToFormat.text = string.Format(TextToFormat.text, ScoreManager.currScore);
        }
    }
}
