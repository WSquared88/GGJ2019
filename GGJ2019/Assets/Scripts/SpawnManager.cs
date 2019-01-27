using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance = null;
    [SerializeField]
    List<GameObject> Spawners;
    [SerializeField]
    List<GameObject> PlayerSpawners;
    int NumAvailableSpawners;
    List<GameObject> UsedSpawners;
    [SerializeField]
    GameObject[] SpawnablePeople;
    [SerializeField]
    int MaxNumSpawnedPeople;
    [SerializeField]
    GameObject[] SpawnableRooms;
    [SerializeField]
    int MaxNumSpawnedRooms;
    [SerializeField]
    InventorySystem PlayerInventory;
    [SerializeField]
    LifeBox GameBoundary;
    [SerializeField]
    GameObject PlayerTemplate;

    public static event Action<GameObject> PlayerRespawned;

    void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Assert(true, "There is more than one instance of the SpawnManager! This shouldn't happen!");
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
        Debug.Assert(Spawners.Count > 0, "There are no spawners in the " + gameObject.name + "!");
        Debug.Assert(PlayerSpawners.Count > 0, "There are no spawners for the player in " + gameObject.name + "!");
        NumAvailableSpawners = Spawners.Count;
        UsedSpawners = new List<GameObject>(NumAvailableSpawners);
        Debug.Assert(SpawnablePeople.Length > 0, "There are no people in the " + gameObject.name + "!");
        Debug.Assert(MaxNumSpawnedPeople > 0, "MaxNumSpawnedPeople on " + gameObject.name + " is set to zero!");
        Debug.Assert(SpawnableRooms.Length > 0, "There are no rooms in the " + gameObject.name + "!");
        Debug.Assert(MaxNumSpawnedRooms > 0, "MaxNumSpawnedRooms in the " + gameObject.name + " is set to zero!");
        Debug.Assert(PlayerInventory, "There isn't an InventorySystem on the " + gameObject.name + "!");
        Debug.Assert(GameBoundary, "There isn't a GameBoundary on the " + gameObject.name + "! We won't be able to subscribe to the killbox events!");
        Debug.Assert(PlayerTemplate, "There isn't a PlayerTemplate on the " + gameObject.name + "!");
        PlayerInventory.SubscribeToPickedUpEvent(HandlePickedUpEvent);
        GameBoundary.SubscribeToDestoryedEvent(HandleDestroyedEvent);

        for(int i = 0;i<MaxNumSpawnedPeople;i++)
        {
            SpawnPerson();
        }

        for(int i = 0;i<MaxNumSpawnedRooms;i++)
        {
            SpawnRoom();
        }
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

    void HandleDestroyedEvent(GameObject destroyed_object)
    {
        PickupComponent pickup = destroyed_object.GetComponent<PickupComponent>();

        //If we're a room or person
        if(pickup)
        {
            RoomPickup room_pickup = pickup as RoomPickup;

            //We're a room
            if(room_pickup)
            {
                SpawnRoom();
            }
            //We're a person
            else
            {
                SpawnPerson();
            }
        }
        //We're probably the player
        else
        {
            House player = destroyed_object.GetComponent<House>();

            if(player)
            {
                SpawnPlayer();
            }
        }
    }

    void SpawnPerson()
    {
        int random_spawner_index = UnityEngine.Random.Range(0, NumAvailableSpawners);
        int random_person_index = UnityEngine.Random.Range(0, SpawnablePeople.Length);

        GameObject spawner = Spawners[random_spawner_index];
        GameObject spawned_person_obj = Instantiate(SpawnablePeople[random_person_index]);
        Person spawned_person_component = spawned_person_obj.GetComponent<Person>();
        spawned_person_component.transform.position = spawner.transform.position;
        spawned_person_component.WanderArea = spawner.GetComponent<Collider>();
        spawned_person_component.HouseObject = PlayerInventory.GetComponent<House>();

        Spawners.Remove(spawner);
        UsedSpawners.Add(spawner);
        NumAvailableSpawners--;
        CheckIfSpawnersRemain();
    }

    void SpawnRoom()
    {
        int random_spawner_index = UnityEngine.Random.Range(0, NumAvailableSpawners);
        int random_room_index = UnityEngine.Random.Range(0, SpawnableRooms.Length);

        GameObject spawner = Spawners[random_spawner_index];
        GameObject spawned_room_obj = Instantiate(SpawnableRooms[random_room_index]);
        RoomPickup spawned_room_component = spawned_room_obj.GetComponent<RoomPickup>();
        spawned_room_component.transform.position = spawner.transform.position;

        Spawners.Remove(spawner);
        UsedSpawners.Add(spawner);
        NumAvailableSpawners--;
        CheckIfSpawnersRemain();
    }

    public void SpawnPlayer()
    {
        int random_spawner_index = UnityEngine.Random.Range(0, PlayerSpawners.Count);

        GameObject spawner = PlayerSpawners[random_spawner_index];
        GameObject spawned_player = Instantiate(PlayerTemplate);
        spawned_player.transform.position = spawner.transform.position;

        PlayerRespawned(spawned_player);
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
