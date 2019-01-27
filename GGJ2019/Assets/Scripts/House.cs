using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// House Management Component
/// </summary>
public class House : MonoBehaviour
{
    public GameObject[] FloorPrefabs;
    private GameObject TopFloor;

	// Use this for initialization
	void Start ()
    {
        TopFloor = gameObject;
	}

    void AddFloor()
    {
        GameObject new_floor = Instantiate<GameObject>(FloorPrefabs[Random.Range(0, FloorPrefabs.Length)]);
        // We need to know the bounding box size of the previous floor and current floor to position properly
        float prev_y_extent = TopFloor.GetComponent<Collider>().bounds.extents.y;
        float cur_y_extent = new_floor.GetComponent<Collider>().bounds.extents.y;

        Vector3 pos = TopFloor.transform.position;
        pos.y += prev_y_extent + cur_y_extent;
        new_floor.transform.position = pos;

        // Next we need to attach the hinge joint to the previous floor and connect it to the new one
        new_floor.transform.parent = TopFloor.transform;
        HingeJoint hj = TopFloor.gameObject.AddComponent<HingeJoint>();
        hj.useSpring = true;
        JointSpring js = hj.spring;
        js.damper = 0.05f;
        js.spring = 3;
        hj.massScale = 1;
        hj.spring = js;
        hj.connectedMassScale = 100;
        hj.connectedBody = new_floor.GetComponent<Rigidbody>();
        JointLimits jl = hj.limits;
        jl.min = 0;
        jl.max = 120;
        jl.bounceMinVelocity = 0.5f;
        jl.contactDistance = 0.1f;
        hj.limits = jl;
        //It's crucial that everything but the base does not restrict rigidbody rotation
        new_floor.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        TopFloor = new_floor;
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
