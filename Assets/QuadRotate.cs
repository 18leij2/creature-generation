using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadRotate : MonoBehaviour
{
    // code to give the quad legs random angles as well
    public Transform axel; // axel object of rotation
    public float minAngle = 0f; // random angles that we rotate the wheel by
    public float maxAngle = -90f;

    void Start()
    {
        // wheel has random angle
        float randomAngle = Random.Range(minAngle, maxAngle);

        // wheel has random scale
        transform.RotateAround(axel.position, axel.right, randomAngle);
    }
}
