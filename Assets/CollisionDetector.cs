using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetector : MonoBehaviour
{
    // create an on trigger event that fires when it collides
    public UnityEvent onTriggerEvent = new UnityEvent();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Leg"))
        {
            Debug.Log("collision occured");

            if (onTriggerEvent != null)
            {
                onTriggerEvent.Invoke();
            }
        }
    }
}
