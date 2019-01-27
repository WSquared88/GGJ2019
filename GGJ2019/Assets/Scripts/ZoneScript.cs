using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneScript : MonoBehaviour
{
    bool SaleAvailable;
    bool makeSale;
    Collider SellArea;

	// Use this for initialization
	void Start ()
    {
        SaleAvailable = false;
        SellArea = GetComponent<BoxCollider>();
        Debug.Assert(SellArea, "There isn't a box collider attached to " + gameObject.name + "!");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Interact"))
        {
            Debug.Log("Sale Made");
            Collider[] hit_objs = Physics.OverlapBox(SellArea.bounds.center, SellArea.bounds.extents);

            for (int i = 0; i < hit_objs.Length; i++)
            {
                PlayerMovement player = hit_objs[i].GetComponent<PlayerMovement>();

                if (player)
                {
                    Rigidbody body = GetComponent<Rigidbody>();

                    if (body)
                    {
                        body.constraints = RigidbodyConstraints.FreezeAll;
                    }

                    ScoreManager.Instance.SellHouse();
                    Destroy(player);
                    SpawnManager.Instance.SpawnPlayer();
                }
            }
            
            //call ScoreManager.SellHouse()
            //fire sale method in house
        }
	}
	void OnTriggerEnter(Collider entrant)
	{
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

