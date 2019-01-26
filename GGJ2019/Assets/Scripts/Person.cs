using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Person component which maintains likes, dislikes, and timer status for People.
/// </summary>
public class Person : MonoBehaviour
{
    public RoomPickup[] Likes;
    public RoomPickup[] Dislikes;

    [SerializeField]
    Sprite PersonUIImage;

    public event Action TimerDepleted;
    private bool TimerDepletedEventFired = false;

    [Tooltip("The maximum value of the timer that this person will use.")]
    public float MaxTimerValue = 10;

    [Tooltip("Used in sell value calculation. This is multiplied by BuyerTime and default Sell Value to get adjusted sell value")]
    public float PercentModifier = 1;


    // TODO the house should inform the Person about its state, and then these values should be calculated based on that.
    private float CurrentLikes = 0;
    private float CurrentDislikes = 1;

    /// <summary>
    /// How much time is left for this Person
    /// </summary>
    public float BuyerTime { get; private set; }

    void Start ()
    {
        BuyerTime = MaxTimerValue;

        // For testing
        TimerDepleted += () => Debug.Log("Timer depleted");
	}

    void ProgressTimer()
    {
        BuyerTime -= Time.deltaTime * CurrentDislikes;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (BuyerTime > 0)
        {
            ProgressTimer();
        }
        else
        {
            if (TimerDepleted != null && !TimerDepletedEventFired)
            {
                TimerDepleted();
                TimerDepletedEventFired = true;
            }
        }

    }

    public Sprite GetPersonUIImage()
    {
        return PersonUIImage;
    }

    public float GetMaxTimerValue()
    {
        return MaxTimerValue;
    }
}
