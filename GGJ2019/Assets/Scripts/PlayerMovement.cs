using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float PlayerSpeed = 1.0f;

	// Use this for initialization
	void Start ()
    {
        Debug.Assert(PlayerSpeed > 0.0f + float.Epsilon, "The player speed is less than zero! This is going to cause weird problems!");
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 move_direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        Rigidbody rigid_body = GetComponent<Rigidbody>();

        if (rigid_body)
        {
            rigid_body.MovePosition(transform.position + move_direction * Time.deltaTime * PlayerSpeed);
        }
	}
}
