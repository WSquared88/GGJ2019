using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collection of autonomous agent behaviours. These return vectors, so it's up to the calling code to use them.
/// </summary>
/// 
[RequireComponent(typeof(Rigidbody))]
public class AutonomousBehaviours : MonoBehaviour
{

    public float MaxSpeed = 10;
    private Rigidbody Rb;

    void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }

    private void StripY(ref Vector3 vec)
    {
        vec.y = 0;
    }

    public Vector3 Seek(Vector3 target)
    {
        Vector3 desired_velocity = (target - transform.position).normalized * MaxSpeed;
        Vector3 steer = desired_velocity - Rb.velocity;
        StripY(ref steer);
        return steer;
    }
    public Vector3 Flee(Vector3 target)
    {
        Vector3 desired_velocity = (transform.position - target).normalized * MaxSpeed;
        Vector3 steer = desired_velocity - Rb.velocity;
        StripY(ref steer);
        return steer;
    }
    public Vector3 Arrive(Vector3 target, float slowingDistance)
    {
        Vector3 target_offset = target - transform.position;
        float distance = target_offset.magnitude;
        float ramped_speed = MaxSpeed * (distance / slowingDistance);
        float clipped_speed = Mathf.Min(ramped_speed, MaxSpeed);
        Vector3 desired_velocity = (clipped_speed / distance) * target_offset;
        Vector3 steer = desired_velocity - Rb.velocity;
        StripY(ref steer);
        return steer;
    }

    public Vector3 Wander(float distanceAhead, float radius)
    {
        Quaternion rotator = Quaternion.Euler(0, Random.Range(-180, Mathf.PerlinNoise(Time.time, 0) * 180), 0);
        Vector3 randOffset = rotator * transform.forward * radius;
        Vector3 target = transform.forward * distanceAhead + randOffset;
        return Seek(transform.position + target);
    }

    public Vector3 Constrain(Bounds bounds)
    {

        /*
        Vector3 min = bounds.center - bounds.extents;
        Vector3 max = bounds.center + bounds.extents;
        Vector3 desired_velocity = Vector3.zero;
        if (transform.position.x < min.x || transform.position.x > max.x)
            desired_velocity.x = -Rb.velocity.x;
        if (transform.position.y < min.y || transform.position.y > max.y)
            desired_velocity.y = -Rb.velocity.y;
        if (transform.position.z < min.z || transform.position.z > max.z)
            desired_velocity.z = -Rb.velocity.z;
        Vector3 steer = desired_velocity - Rb.velocity;
        StripY(ref steer);*/
        if (!bounds.Contains(transform.position))
            return Seek(bounds.center);
        else
            return Vector3.zero;
    }

}
