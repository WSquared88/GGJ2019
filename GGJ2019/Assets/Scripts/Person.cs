﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Person component which maintains likes, dislikes, and timer status for People.
/// </summary>
public class Person : MonoBehaviour
{
    public RoomPickup[] Likes;
    public RoomPickup[] Dislikes;

    public event Action TimerDepleted;
    private bool TimerDepletedEventFired = false;

    [Tooltip("The maximum value of the timer that this person will use.")]
    public float MaxTimerValue = 10;

    [Tooltip("Value that will be multiplied by DeltaTime and subtracted from the timer each frame.")]
    public float TimerDecrementMultiplier = 1; 

    private float Timer;

    void Start ()
    {
        Timer = MaxTimerValue;

        // For testing
        TimerDepleted += () => Debug.Log("Timer depleted");
	}

    void ProgressTimer()
    {
        Timer -= Time.deltaTime * TimerDecrementMultiplier;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Timer > 0)
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
}