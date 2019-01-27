using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// House Management Component
/// </summary>
public class House : MonoBehaviour
{
    public GameObject[] FloorPrefabs;
    public GameObject BasePrefab;
    public Camera PlayerCam;
    Animator conductor;
    private GameObject TopFloor;
    private float SyncTime;
	// Use this for initialization
	void Awake ()
    {
        GetComponent<InventorySystem>().SubscribeToPickedUpEvent((PickupComponent pc) =>
        {
            RoomPickup rp = pc as RoomPickup;
            if (rp != null && rp.GetRoomType() == RoomTypes.Floor)
            {
                AddFloor();
            }
        });
	}

    void AddFloor()
    {
        GameObject new_floor;
        if (transform.childCount > 0)
        {
            new_floor = Instantiate<GameObject>(FloorPrefabs[Random.Range(0, FloorPrefabs.Length)]);
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
            js.spring = 2;
            hj.massScale = 1;
            hj.spring = js;
            hj.connectedMassScale = 750;
            hj.connectedBody = new_floor.GetComponent<Rigidbody>();
            JointLimits jl = hj.limits;
            jl.min = 0;
            jl.max = 120;
            jl.bounceMinVelocity = 0.5f;
            jl.contactDistance = 0.1f;
            hj.limits = jl;
            //It's crucial that everything but the base does not restrict rigidbody rotation
            new_floor.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        else
        {
            new_floor = Instantiate<GameObject>(BasePrefab);
            new_floor.transform.parent = transform;
            new_floor.transform.GetComponent<PlayerMovement>().SetPlayerCamera(PlayerCam);
            PlayerCam.GetComponent<PlayerCamera>().SetTarget(new_floor.transform);
            GetComponent<InventorySystem>().SetTarget(new_floor.transform);
            conductor = new_floor.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        }

        GetBaseAnimFrame();
        new_floor.GetComponentInChildren<Animator>().Play("Take 001", -1, SyncTime);
        TopFloor = new_floor;


        // After animation has played, update position if it is root
        if (transform.childCount == 1)
        {
            new_floor.transform.localPosition = Vector3.zero;
        }
    }

	void GetBaseAnimFrame()
    {
        AnimatorStateInfo asi = conductor.GetCurrentAnimatorStateInfo(0);
        SyncTime = asi.normalizedTime;
        // conductor.animation["Take 001"].time

        //SyncTime = conductor.
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
