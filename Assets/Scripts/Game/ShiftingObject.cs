using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftingObject : MonoBehaviour
{
    public Vector3 direction = Vector3.forward; 
    public float speed = 1.0f; 
    public float duration = 5.0f; 

    private float elapsedTime = 0.0f; 

    void Update()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= duration)
        { 
            enabled = false;
        }
    }
}