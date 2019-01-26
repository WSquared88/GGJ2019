using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Person component which maintains likes, dislikes, and timer status for People.
/// </summary>
[RequireComponent(typeof(AutonomousBehaviours))]
public class Person : MonoBehaviour
{
    public RoomPickup[] Likes;
    public RoomPickup[] Dislikes;

    public event Action TimerDepleted;
    private bool TimerDepletedEventFired = false;

    [Tooltip("The maximum value of the timer that this person will use.")]
    public float MaxTimerValue = 10;

    [Tooltip("Used in sell value calculation. This is multiplied by BuyerTime and default Sell Value to get adjusted sell value")]
    public float PercentModifier = 1;

    public Collider WanderArea;

    // TODO the house should inform the Person about its state, and then these values should be calculated based on that.
    private float CurrentLikes = 0;
    private float CurrentDislikes = 1;

    /// <summary>
    /// How much time is left for this Person
    /// </summary>
    public float BuyerTime { get; private set; }

    private AutonomousBehaviours AutoBehav;
    private Rigidbody Rb;

    public House HouseObject;

    private Action AIFunction;
    void Start ()
    {
        BuyerTime = MaxTimerValue;
        Rb = GetComponent<Rigidbody>();
        AutoBehav = GetComponent<AutonomousBehaviours>();
        // For testing
        TimerDepleted += () => Debug.Log("Timer depleted");
        Debug.Assert(HouseObject, "House must be attached");
        AIFunction = ConstrainedWander;
	}

    #region AI
    void ConstrainedWander()
    {
        transform.forward = Rb.velocity.normalized;
        Rb.AddForce(AutoBehav.Wander(20, 5));
        Rb.AddForce(AutoBehav.Constrain(WanderArea.bounds) * 5);
    }

    void ArriveAtHouse()
    {
        transform.forward = Rb.velocity.normalized;
        Rb.AddForce(AutoBehav.Arrive(HouseObject.transform.position, 20));
    }

    void FleeFromhouse()
    {
        transform.forward = Rb.velocity.normalized;
        Rb.AddForce(AutoBehav.Flee(HouseObject.transform.position));
    }

    #endregion

    void ProgressTimer()
    {
        BuyerTime -= Time.deltaTime * CurrentDislikes;
    }
	
	// Update is called once per frame
	void Update ()
    {
        AIFunction();
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
}
