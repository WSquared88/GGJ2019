using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Person component which maintains likes, dislikes, and timer status for People.
/// </summary>
public class Person : MonoBehaviour
{
    // Temporary class until pickup class is created
    public class RoomPickup
    {

    }

    public List<RoomPickup> Likes { get; private set; }
    public List<RoomPickup> Dislikes { get; private set; }
    public event Action TimerDepleted;
    [Tooltip("The maximum value of the timer that this person will use.")]
    public float MaxTimerValue = 10;
    [Tooltip("Value that will be multiplied by DeltaTime and subtracted from the timer each frame.")]
    public float TimerDecrementMultiplier = 1; 
    private float Timer;
    private bool TimerDepletedEventFired = false;

    void Start ()
    {
        Timer = MaxTimerValue;
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
