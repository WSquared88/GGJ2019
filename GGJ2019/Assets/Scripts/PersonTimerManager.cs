using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonTimerManager : MonoBehaviour
{
    [SerializeField]
    Image Timer;
    [SerializeField]
    Image PersonImage;

	// Use this for initialization
	void Start ()
    {
        Debug.Assert(Timer, "The timer image wasn't set on the PersonTimer prefab!");
        Timer.fillAmount = 1;
    }
	
    public void SetPersonImage(Sprite new_image)
    {
        PersonImage.sprite = new_image;
    }

    public void SetFillAmount(float amount)
    {
        Timer.fillAmount = amount;
    }
}
