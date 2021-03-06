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
    public Vector3 localOffset;

    private Vector3 TargetToCam;

	// Use this for initialization
	void Start ()
    {
        if (Target != null)
        {
            //TargetToCam = (transform.position - Target.position);

        }
        SpawnManager.PlayerRespawned += PlayerRespawnedHandler;

    }

    public void SetTarget(Transform target)
    {
        Target = target;
        TargetToCam = (transform.position - Target.position);
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
		if (FollowTarget)
        {
            Vector3 offset = transform.TransformDirection(localOffset);
            transform.position = Vector3.Lerp(transform.position, Target.transform.position + offset, Smooth * Time.fixedDeltaTime);
        }
	}

    void PlayerRespawnedHandler(GameObject new_player)
    {
        Target = new_player.transform;
    }
}
