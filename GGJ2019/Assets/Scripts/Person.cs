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
    public RoomTypes[] Likes;
    public RoomTypes[] Dislikes;

    public event Action TimerDepleted;
    private bool TimerDepletedEventFired = false;

    [Tooltip("The maximum value of the timer that this person will use.")]
    public float MaxTimerValue = 10;

    [Tooltip("Used in sell value calculation. This is multiplied by BuyerTime and default Sell Value to get adjusted sell value")]
    public float PercentModifier = 1;

    [Tooltip("Area where the collider is allowed to wander, represented by a collider")]
    public Collider WanderArea;


    private int CurrentLikes
    {
        get
        {
            int likes = 0;
            foreach (RoomTypes type in Likes)
            {
                if (HouseInventory.GetRoomCount(type) > 0)
                {
                    likes++;
                }
            }
            return likes;
        }
    }
    private int CurrentDislikes
    {
        get
        {
            int dislikes = 0;
            foreach (RoomTypes type in Dislikes)
            {
                if (HouseInventory.GetRoomCount(type) > 0)
                {
                    dislikes++;
                }
            }
            return dislikes;
        }
    }

    /// <summary>
    /// How much time is left for this Person
    /// </summary>
    public float BuyerTime { get; private set; }

    private AutonomousBehaviours AutoBehav;
    private Rigidbody Rb;

    public House HouseObject;
    private InventorySystem HouseInventory;

    [Tooltip("If house's distance to this person is less than this, the person may seek or flee the house")]
    public float MinHouseDistance = 20;

    private Action AIFunction;
    void Start ()
    {
        BuyerTime = MaxTimerValue;
        Rb = GetComponent<Rigidbody>();
        AutoBehav = GetComponent<AutonomousBehaviours>();
        Debug.Assert(HouseObject, "House must be attached");
        HouseInventory = HouseObject.GetComponent<InventorySystem>();
        AIFunction = ConstrainedWander;
	}

    #region AI
    void ConstrainedWander()
    {
        transform.forward = Rb.velocity.normalized;
        Rb.AddForce(AutoBehav.Wander(20, 5));
        Rb.AddForce(AutoBehav.Constrain(WanderArea.bounds) * 5);
        float houseDistanceSqr = (transform.position - HouseObject.transform.position).sqrMagnitude;
        // Cache likes and dislikes
        int likes = CurrentLikes;
        int dislikes = CurrentDislikes;
        if (houseDistanceSqr < Math.Pow(MinHouseDistance, 2))
        {
            if (likes > dislikes)
                AIFunction = ArriveAtHouse;
            else if (likes < dislikes)
                AIFunction = FleeFromhouse;
        }
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
        float houseDistanceSqr = (transform.position - HouseObject.transform.position).sqrMagnitude;
        if (houseDistanceSqr > Math.Pow(MinHouseDistance * 2, 2))
        {
            AIFunction = ConstrainedWander;
        }
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
