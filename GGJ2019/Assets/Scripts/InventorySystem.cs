using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    List<PickupComponent> Items;
    [SerializeField]
    float PickupRadius;
    [SerializeField]
    bool DebugDrawCollision;
    public event Action<PickupComponent> ItemPickedUp;


	// Use this for initialization
	void Start ()
    {
        Items = new List<PickupComponent>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetButtonDown("Interact"))
        {
            Collider[] hit_collider = Physics.OverlapSphere(gameObject.transform.position, PickupRadius);

            for (int i = 0; i < hit_collider.Length; i++)
            {
                PickupComponent hit_pickup = hit_collider[i].gameObject.GetComponent<PickupComponent>();
                if (hit_pickup != null)
                {
                    AddItem(hit_pickup);
                }
            }
        }
	}

    void OnDrawGizmos()
    {
        if (Debug.isDebugBuild && DebugDrawCollision)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(gameObject.transform.position, PickupRadius);
        }
    }

    public void AddItem(PickupComponent item)
    {
        Debug.Log("We picked up the item! It was really cool!");
        item.GetComponent<Renderer>().enabled = false;
        item.GetComponent<Collider>().enabled = false;
        Items.Add(item);

        if (ItemPickedUp != null)
        {
            ItemPickedUp(item);
        }
    }

    public int GetRoomCount(RoomTypes room_type)
    {
        int count = 0;

        for (int i = 0; i < Items.Count; i++)
        {
            RoomPickup room_pickup_component = Items[i] as RoomPickup;

            if (room_pickup_component != null && room_pickup_component.GetRoomType() == room_type)
            {
                count++;
            }
        }

        return count;
    }

    public void SubscribeToPickedUpEvent(Action<PickupComponent> subscribing_function)
    {
        ItemPickedUp += subscribing_function;
    }
}
