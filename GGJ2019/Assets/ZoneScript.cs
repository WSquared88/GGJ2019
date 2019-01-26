using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneScript : MonoBehaviour {

    bool SaleAvailable;
    bool makeSale;
	// Use this for initialization
	void Start () {
        SaleAvailable = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (SaleAvailable && Input.GetButtonDown("Interact"))
        {
            Debug.Log("Sale Made");
            makeSale = true;
            //fire sale method in house
        }
        else
        {
            makeSale = false;
        }
	}
	void OnTriggerEnter(Collider entrant)
	{
        //foreach(ContactPoint contact in collision.contacts)
        //{};
        if (entrant.gameObject.tag == "house")
        {
                Debug.Log("Sale available");
            SaleAvailable = true;
        }
	}
    private void OnTriggerExit(Collider entrant)
    {
        if (entrant.gameObject.tag == "house")
        { 
                Debug.Log("Explore available");
            SaleAvailable = false;
        }
    }

}

