using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBox : MonoBehaviour
{
    public event Action<GameObject> ItemDestroyed;

    void OnTriggerExit(Collider other)
    {
        KillTarget(other.gameObject);
    }

    void OnCollisionExit(Collision other)
    {
        KillTarget(other.gameObject);
    }

    void KillTarget(GameObject other)
    {
        Debug.Log("We killed the target for leaving home!");
        ItemDestroyed(other.gameObject);

        Destroy(other.gameObject);
    }

    public void SubscribeToDestoryedEvent(Action<GameObject> event_handler)
    {
        ItemDestroyed += event_handler;
    }
}
