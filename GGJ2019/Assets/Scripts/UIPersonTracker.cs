using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPersonTracker : MonoBehaviour
{
    List<PersonTimerManager> PersonTimerManagers;
    List<Person> CurrentPeopleComponents;
    [SerializeField]
    InventorySystem PlayerInventory;
    [SerializeField]
    GameObject PersonTimerTemplate;

	// Use this for initialization
	void Start ()
    {
        PersonTimerManagers = new List<PersonTimerManager>();
        CurrentPeopleComponents = new List<Person>();
        Debug.Assert(PlayerInventory != null, "The player inventory wasn't set on the UIPersonTracker component of the UI!");
        Debug.Assert(PersonTimerTemplate, "The slider for the person timer wasn't set on the UIPersonTracker component of the UI!");
        PlayerInventory.SubscribeToPickedUpEvent(SpawnNewPersonUI);
        SpawnManager.PlayerRespawned += PlayerRespawnedHandler;
	}
	
	// Update is called once per frame
	void Update ()
    {
        for(int i = 0;i<PersonTimerManagers.Count;i++)
        {
            Person person = CurrentPeopleComponents[i];
            PersonTimerManagers[i].SetFillAmount(person.BuyerTime / person.GetMaxTimerValue());
        }
	}

    void SpawnNewPersonUI(PickupComponent pickup)
    {
        Person person_component = pickup.GetComponent<Person>();

        if(person_component)
        {
            GameObject person_timer_obj = Instantiate(PersonTimerTemplate, transform);
            PersonTimerManager time_manager = person_timer_obj.GetComponent<PersonTimerManager>();
            time_manager.SetPersonImage(person_component.GetPersonUIImage());
            CurrentPeopleComponents.Add(person_component);
            PersonTimerManagers.Add(time_manager);
        }
    }

    void PlayerRespawnedHandler(GameObject new_player)
    {
        for (int i = 0; i < PersonTimerManagers.Count; i++)
        {
            Destroy(PersonTimerManagers[i].gameObject);
        }

        PersonTimerManagers.Clear();
        CurrentPeopleComponents.Clear();
        PlayerInventory = new_player.GetComponent<InventorySystem>();
    }
}
