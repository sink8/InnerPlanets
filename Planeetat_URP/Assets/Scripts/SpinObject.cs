using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up; // Default rotation axis
    public float rotationSpeed = 30f; // Default rotation speed

    void Update()
    {
        // Rotate the object around the specified axis
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }
}
