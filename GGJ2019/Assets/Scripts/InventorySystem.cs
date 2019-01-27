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
    int MaxNumRoomsPerFloor = 3;
    [SerializeField]
    GameObject[] AutoActivatedPickupComponents;
    [SerializeField]
    bool DebugDrawCollision;
    [SerializeField]
    Transform Target;
    public event Action<PickupComponent> ItemPickedUp;

    public List<Person> People
    {
        get
        {
            List<Person> people = new List<Person>();
            foreach (PickupComponent item in Items)
            {
                Person person = item.GetComponent<Person>();
                if (person != null)
                {
                    people.Add(person);
                }
            }
            return people;
        }
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }

	// Use this for initialization
	void Start ()
    {
        Items = new List<PickupComponent>();
        Person.TimerDepleted += PersonLeftHandler;

        foreach (GameObject item in AutoActivatedPickupComponents)
        {
            GameObject instantiated_item = Instantiate(item);
            PickupComponent auto_pickup_component = instantiated_item.GetComponent<PickupComponent>();
            CheckCanAddItem(auto_pickup_component);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Target == null)
        {
            return;
        }
		if(Input.GetButtonDown("Interact"))
        {
            Collider[] hit_collider = Physics.OverlapSphere(Target.transform.position, PickupRadius);

            for (int i = 0; i < hit_collider.Length; i++)
            {
                PickupComponent hit_pickup = hit_collider[i].gameObject.GetComponent<PickupComponent>();
                if (hit_pickup != null)
                {
                    CheckCanAddItem(hit_pickup);
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

    void CheckCanAddItem(PickupComponent item)
    {
        RoomPickup room_pickup = item as RoomPickup;

        //If we're a room piece
        if(room_pickup)
        {
            //If we're a floor we always want to be picked up
            if (room_pickup.GetRoomType() == RoomTypes.Floor)
            {
                AddItem(room_pickup);
            }
            //We must be a room
            else
            {
                int num_floors = GetRoomCount(RoomTypes.Floor);
                int num_non_floor_rooms = GetCountAllRooms() - num_floors;

                //We only want to be added if we have enough floors for the items
                if (num_non_floor_rooms < num_floors * MaxNumRoomsPerFloor)
                {
                    AddItem(room_pickup);
                }
            }
        }
        //If we're just a person
        else
        {
            Person person = item.GetComponent<Person>();
            if (person)
            {
                person.StartTimer();
                person.DisableRenderers();

            }
            AddItem(item);
        }
    }

    void AddItem(PickupComponent item)
    {
        Debug.Log("We picked up the item! It was really cool!");
        // items may not have renderers at root level
        Renderer rend = item.GetComponent<Renderer>();
        if (rend)
        {
            rend.enabled = false;
        }
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

        if (Items != null)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                RoomPickup room_pickup_component = Items[i] as RoomPickup;

                if (room_pickup_component != null && room_pickup_component.GetRoomType() == room_type)
                {
                    count++;
                }
            }
        }

        return count;
    }

    public int GetCountAllRooms()
    {
        return Items.Count;
    }

    public int GetMaxNumRoomsPerFloor()
    {
        return MaxNumRoomsPerFloor;
    }

    public void SubscribeToPickedUpEvent(Action<PickupComponent> subscribing_function)
    {
        ItemPickedUp += subscribing_function;
    }

    void PersonLeftHandler(GameObject person_obj)
    {
        for(int i = 0;i<Items.Count;i++)
        {
            if(Items[i].gameObject == person_obj)
            {
                Items.RemoveAt(i);
                break;
            }
        }
    }
}
