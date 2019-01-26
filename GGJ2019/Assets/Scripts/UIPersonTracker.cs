using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPersonTracker : MonoBehaviour
{
    List<Person> CurrentPeopleComponents;
    List<Image> PeopleImages;
    List<GameObject> PeopleSliders;
    [SerializeField]
    InventorySystem PlayerInventory;
    [SerializeField]
    GameObject PersonTimerTemplate;

	// Use this for initialization
	void Start ()
    {
        CurrentPeopleComponents = new List<Person>();
        PeopleImages = new List<Image>();
        PeopleSliders = new List<GameObject>();
        Debug.Assert(PlayerInventory != null, "The player inventory wasn't set on the UIPersonTracker component of the UI!");
        Debug.Assert(PersonTimerTemplate, "The slider for the person timer wasn't set on the UIPersonTracker component of the UI!");
        PlayerInventory.SubscribeToPickedUpEvent(SpawnNewPersonUI);
	}
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 0; i < CurrentPeopleComponents.Count && i < PeopleImages.Count && i < PeopleSliders.Count; i++)
        {
            Person person = CurrentPeopleComponents[i];
            Image slider_image = PeopleSliders[i].GetComponentInChildren<Image>();

            if (slider_image)
            {
                slider_image.fillAmount = person.BuyerTime / person.GetMaxTimerValue();
            }
        }
	}

    void SpawnNewPersonUI(PickupComponent pickup)
    {
        Person person_component = pickup.GetComponent<Person>();

        if(person_component)
        {
            CurrentPeopleComponents.Add(person_component);
            GameObject person_image_obj = new GameObject();
            Image person_image = person_image_obj.AddComponent<Image>();
            person_image.sprite = person_component.GetPersonUIImage();
            RectTransform person_image_rect_transform = person_image_obj.GetComponent<RectTransform>();
            person_image_rect_transform.SetParent(transform);
            person_image_rect_transform.localPosition = new Vector3(0, 0, 0);
            person_image_obj.SetActive(true);
            PeopleImages.Add(person_image);

            GameObject person_timer_obj = Instantiate(PersonTimerTemplate, transform);
            PeopleSliders.Add(person_timer_obj);
        }
    }
}
