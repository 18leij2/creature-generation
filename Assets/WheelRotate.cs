using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotate : MonoBehaviour
{
    public Transform axel; // axel object of rotation
    public float minAngle = 90f; // random angles that we rotate the wheel by
    public float maxAngle = -90f;

    private Transform wheel;

    void Start()
    {
        // wheel has random angle
        float randomAngle = Random.Range(minAngle, maxAngle);

        transform.RotateAround(axel.position, axel.right, randomAngle);

        wheel = transform.GetChild(0);

        // wheel has random scale
        float randomScale = Random.Range(3f, 6f);
        wheel.localScale = new Vector3(randomScale, wheel.localScale.y, wheel.localScale.z);
    }
}
