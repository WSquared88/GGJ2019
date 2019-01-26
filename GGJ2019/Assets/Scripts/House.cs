using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class House : MonoBehaviour
{
    public Floor[] FloorPrefabs;


	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < 10; ++i)
           AddFloor();
	}

    void AddFloor()
    {
        Floor new_floor = Instantiate<Floor>(FloorPrefabs[Random.Range(0, FloorPrefabs.Length)]);
        new_floor.transform.parent = this.transform;
        if (transform.childCount == 1)
        {
            new_floor.transform.position = Vector3.zero;
        }
        else
        {
            // We need to know the bounding box size of the previous floor and current floor to position properly
            Transform prev_floor = transform.GetChild(transform.childCount - 2);
            float prev_y_extent = prev_floor.GetComponent<MeshRenderer>().bounds.extents.y;
            float cur_y_extent = new_floor.GetComponent<MeshRenderer>().bounds.extents.y;

            Vector3 pos = prev_floor.transform.position;
            pos.y += prev_y_extent + cur_y_extent;
            new_floor.transform.position = pos;
        }
    }

    Floor GetFloor(int index)
    {
        Debug.Assert(index >= 0 && index < transform.childCount, "Invalid floor index");
        return transform.GetChild(index).GetComponent<Floor>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
