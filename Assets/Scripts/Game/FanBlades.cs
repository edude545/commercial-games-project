using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlades : MonoBehaviour
{
    // Adjust this speed to control how fast the blades spin
    public float rotationSpeed = 100f;

    void Update()
    {
        // Rotate the fan blades around the Z-axis
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}