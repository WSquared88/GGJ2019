using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// House Management Component
/// </summary>
public class House : MonoBehaviour
{
    public GameObject[] FloorPrefabs;

	// Use this for initialization
	void Start ()
    {
        AddFloor();
	}

    void AddFloor()
    {
        GameObject new_floor = Instantiate<GameObject>(FloorPrefabs[Random.Range(0, FloorPrefabs.Length)]);
        new_floor.transform.parent = this.transform;
        if (transform.childCount == 1)
        {
            new_floor.transform.localPosition = Vector3.zero;
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

	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            AddFloor();
        }
	}
}
