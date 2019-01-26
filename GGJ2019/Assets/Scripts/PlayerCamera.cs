﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera which will move towards and keep the target centered in view
/// </summary>
public class PlayerCamera : MonoBehaviour {

    public Transform Target;
    public float Smooth = 2f;
    public bool FollowTarget = true;

    private Vector3 TargetToCam;

	// Use this for initialization
	void Start ()
    {
        Debug.Assert(Target != null, "No Target has been set");
        TargetToCam = (transform.position - Target.position);
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
		if (FollowTarget)
        {
            transform.position = Vector3.Lerp(transform.position, Target.transform.position + TargetToCam, Smooth * Time.fixedDeltaTime);
        }
	}
}