using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> Spawners;
    int NumAvailableSpawners;
    List<GameObject> UsedSpawners;
    [SerializeField]
    Person[] SpawnablePeople;
    [SerializeField]
    int MaxNumSpawnedRooms;
    [SerializeField]
    RoomPickup[] SpawnableRooms;
    [SerializeField]
    int MaxNumSpawnedPeople;
    [SerializeField]
    InventorySystem PlayerInventory;

	// Use this for initialization
	void Start ()
    {
        Debug.Assert(Spawners.Count > 0, "There are no spawners in the " + gameObject.name + "!");
        NumAvailableSpawners = Spawners.Count;
        UsedSpawners = new List<GameObject>(NumAvailableSpawners);
        Debug.Assert(SpawnablePeople.Length > 0, "There are no people in the " + gameObject.name + "!");
        Debug.Assert(MaxNumSpawnedPeople > 0, "MaxNumSpawnedPeople on " + gameObject.name + " is set to zero!");
        Debug.Assert(SpawnableRooms.Length > 0, "There are no rooms in the " + gameObject.name + "!");
        Debug.Assert(MaxNumSpawnedRooms > 0, "MaxNumSpawnedRooms in the " + gameObject.name + " is set to zero!");
        Debug.Assert(PlayerInventory, "There isn't an InventorySystem on the " + gameObject.name + "!");
        PlayerInventory.SubscribeToPickedUpEvent(HandlePickedUpEvent);

        for(int i = 0;i<MaxNumSpawnedPeople;i++)
        {
            SpawnPerson();
        }

        for(int i = 0;i<MaxNumSpawnedRooms;i++)
        {
            SpawnRoom();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void HandlePickedUpEvent(PickupComponent pickup_component)
    {
        RoomPickup room_pickup = pickup_component as RoomPickup;

        //If we're a room
        if (room_pickup)
        {
            SpawnRoom();
        }
        //If we're a person
        else
        {
            SpawnPerson();
        }
    }

    void SpawnPerson()
    {
        int random_spawner_index = Random.Range(0, NumAvailableSpawners);
        int random_person_index = Random.Range(0, SpawnablePeople.Length);

        GameObject spawner = Spawners[random_spawner_index];
        Person spawned_person = Instantiate(SpawnablePeople[random_person_index]);
        spawned_person.transform.position = spawner.transform.position;
        spawned_person.WanderArea = spawner.GetComponent<Collider>();
        spawned_person.HouseObject = PlayerInventory.GetComponent<House>();

        Spawners.Remove(spawner);
        UsedSpawners.Add(spawner);
        NumAvailableSpawners--;
        CheckIfSpawnersRemain();
    }

    void SpawnRoom()
    {
        int random_spawner_index = Random.Range(0, NumAvailableSpawners);
        int random_room_index = Random.Range(0, SpawnableRooms.Length);

        GameObject spawner = Spawners[random_spawner_index];
        RoomPickup spawned_room = Instantiate(SpawnableRooms[random_room_index], spawner.transform);
        spawned_room.transform.position = spawner.transform.position;

        Spawners.Remove(spawner);
        UsedSpawners.Add(spawner);
        NumAvailableSpawners--;
        CheckIfSpawnersRemain();
    }

    void CheckIfSpawnersRemain()
    {
        if(NumAvailableSpawners <= 0)
        {
            Spawners.AddRange(UsedSpawners);
            UsedSpawners.Clear();
            NumAvailableSpawners = Spawners.Count;
        }
    }
}
