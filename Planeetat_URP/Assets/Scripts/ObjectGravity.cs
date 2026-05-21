using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGravity : MonoBehaviour
{
    public float gravityStrength = 9.81f; // Strength of gravity, adjust as needed
    public Transform planetCenter;
    void FixedUpdate()
    {
        if (planetCenter == null)
        {
            Debug.LogError("Please assign the planet center to the ObjectGravity script.");
            return;
        }

        // Calculate the direction towards the center of the planet
        Vector3 directionToPlanetCenter = (planetCenter.position - transform.position).normalized;

        // Apply gravitational force towards the center of the planet
        GetComponent<Rigidbody>().AddForce(directionToPlanetCenter * gravityStrength, ForceMode.Acceleration);
    }
}
