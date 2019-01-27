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
    [SerializeField]
    Vector3 ImageOffset;
    [SerializeField]
    Vector2 ImageSize;

	// Use this for initialization
	void Start ()
    {
        PersonTimerManagers = new List<PersonTimerManager>();
        CurrentPeopleComponents = new List<Person>();
        Debug.Assert(PlayerInventory != null, "The player inventory wasn't set on the UIPersonTracker component of the UI!");
        Debug.Assert(PersonTimerTemplate, "The slider for the person timer wasn't set on the UIPersonTracker component of the UI!");
        PlayerInventory.SubscribeToPickedUpEvent(SpawnNewPersonUI);
        SpawnManager.PlayerRespawned += PlayerRespawnedHandler;
        Person.TimerDepleted += PersonLeftHandler;
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
            //The person tracker is a group of UI elements so we want to get and modify the sizeDelta of all of them
            RectTransform person_timer_transform = person_timer_obj.GetComponent<RectTransform>();
            RectTransform[] person_timer_child_transforms = person_timer_obj.GetComponentsInChildren<RectTransform>();

            for (int i = 0; i < person_timer_child_transforms.Length; i++)
            {
                person_timer_child_transforms[i].sizeDelta = ImageSize;
            }

            person_timer_transform.localPosition = ImageOffset * PersonTimerManagers.Count;
            PersonTimerManager time_manager = person_timer_obj.GetComponent<PersonTimerManager>();
            time_manager.SetPersonImage(person_component.GetPersonUIImage());
            CurrentPeopleComponents.Add(person_component);
            PersonTimerManagers.Add(time_manager);
        }
    }

    void MoveUIDown(int removed_index)
    {
        for(int i = removed_index;i<PersonTimerManagers.Count;i++)
        {
            RectTransform rect_transform = PersonTimerManagers[i].GetComponent<RectTransform>();
            rect_transform.localPosition -= ImageOffset;
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

    void PersonLeftHandler(GameObject person_obj)
    {
        for(int i = 0;i<CurrentPeopleComponents.Count;i++)
        {
            if(CurrentPeopleComponents[i].gameObject == person_obj)
            {
                Destroy(PersonTimerManagers[i].gameObject);
                CurrentPeopleComponents.RemoveAt(i);
                PersonTimerManagers.RemoveAt(i);
                MoveUIDown(i);
                break;
            }
        }
    }
}
