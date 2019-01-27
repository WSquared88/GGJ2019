using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotFlyIntoCamera : MonoBehaviour
{

    public float Duration = 4;
    public Camera TargetCamera;
    public event Action TransitionComplete;
    private float StartTime;
    private bool IsFlying = false;
    private Vector3 StartPos;
    private Quaternion StartRot;

	// Use this for initialization
	void Start ()
    {
        Debug.Assert(TargetCamera, "Target Camera must be assigned");
	}

    public void Fly()
    {
        StartTime = Time.time;
        StartPos = transform.position;
        StartRot = transform.rotation;
        IsFlying = true;
    }

    /// <summary>
    /// Courtesy of https://stackoverflow.com/questions/13462001/ease-in-and-ease-out-animation-formula
    /// </summary>
    private float EaseInOut(float t)
    {
        if (t <= 0.5f)
        {
            return 2.0f * Mathf.Pow(t, 2);
        }
        else
        {
            t -= 0.5f;
            return 2.0f * t * (1.0f - t) + 0.5f;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (IsFlying)
        {
            float time = Time.time - StartTime;
            time /= Duration;
            float ease_time = EaseInOut(time);
            transform.position = Vector3.Lerp(StartPos, TargetCamera.transform.position, ease_time);
            transform.rotation = Quaternion.Slerp(StartRot, TargetCamera.transform.rotation, ease_time);
            if (time >= 1)
            {
                TargetCamera.enabled = true;
                IsFlying = false;
                if (TransitionComplete != null)
                {
                    TransitionComplete();
                }
                this.GetComponent<Camera>().enabled = false;
            }
        }
	}
}
