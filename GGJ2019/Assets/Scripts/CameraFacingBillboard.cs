using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour
{
    public Camera CameraToLookAt;

    void Start()
    {
        if (CameraToLookAt == null)
        {
            CameraToLookAt = Camera.main;
        }
    }

    /// <summary>
    /// Courtesy of http://wiki.unity3d.com/index.php/CameraFacingBillboard
    /// </summary>
    void LateUpdate()
    {
        if (CameraToLookAt)
        {
            transform.LookAt(transform.position + CameraToLookAt.transform.rotation * Vector3.forward,
                CameraToLookAt.transform.rotation * Vector3.up);
        }

    }
}
