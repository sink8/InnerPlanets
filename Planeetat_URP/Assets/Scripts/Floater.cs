using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public float waveAmplitude = 1f; // how high the wave is
    public float waveSpeed = 1f; // How fast the wave is

    float startingYPositiony; // Holds the original y-position value
    float startingYPositionx; // Holds the original y-position value

    // Start is called before the first frame update
    void Start()
    {
        // Store the original y-position value
        startingYPositiony = transform.position.y;
        startingYPositionx = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        // Create and assign a new position -vector every frame,
        // where the y -value is calculated by adding the original value to a sine -function
        transform.position = new Vector3(
            startingYPositionx + waveAmplitude * Mathf.Sin(waveSpeed * Time.time),
            startingYPositiony + waveAmplitude * Mathf.Sin(waveSpeed * Time.time),
            transform.position.z);
    }
}
