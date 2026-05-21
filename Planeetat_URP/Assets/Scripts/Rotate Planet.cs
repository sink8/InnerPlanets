using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlanet : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up; // Axis of rotation
    public float rotationSpeed = 90f; // Speed of rotation in degrees per second
    public float rotationamount = 90f;
    public bool isRotating = false; 

    void Start()
    {
        //StartRotation();
    }

    
    void Update()
    {
        if(isRotating == true)
        {
            RotateObject();
        }

    }

    public void RotateObject()
    {
        // Calculate the amount of rotation per frame based on rotation speed and frame time
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Rotate the object around the specified axis by the calculated amount
        transform.Rotate(rotationAxis, rotationAmount);

        // Check if rotation by 90 degrees has been reached
        if (Mathf.Abs(transform.rotation.eulerAngles.y % 360) >= 90)
        {
            // Stop rotating when 90 degrees has been reached
            StopRotation();

            //    var rotationAmount = rotationSpeed * Time.deltaTime;
            //transform.Rotate(rotationAxis, rotationAmount);
        }
    }

        IEnumerator RotateXDegrees()
        {
            isRotating = true;
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationAxis * rotationamount);

            while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                yield return null;
            }
            isRotating = false;
        }

        public void StartRotation()
        {
            StartCoroutine(RotateXDegrees());
        }

        void StopRotation()
        {
            StopAllCoroutines();
            isRotating = false;
        }
    
}
