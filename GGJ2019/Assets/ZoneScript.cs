using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider entrant)
	{
        //foreach(ContactPoint contact in collision.contacts)
        //{};
        if (entrant.gameObject.tag == "house")
        {
            Debug.Log("Collision Entered");
        }
	}
	
}

