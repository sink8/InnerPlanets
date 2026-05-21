using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundPlanet : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 3;
    public GameObject pivotPoint;
    public GameObject planet; // Reference to the planet1 object
    public PuzzlleTriggers puzzle;


    public float riseThreshold = 10f; // Height at which rotation starts
    public bool canRotate = false;
    public bool hasRisen = false;

    public float riseSpeed = 1f;
    public float maxRiseHeight = 10f;
    public Vector3 riseDirection = Vector3.up;
    public Vector3 rotationAxis = Vector3.up;

    void Start()
    {
        planet = GameObject.Find("planeetta_yläkehä");
        GameObject g = GameObject.Find("CloudsChanger");
        puzzle = g.GetComponent<PuzzlleTriggers>();
        rotationAxis = puzzle.cloudsRotationAxis;
        Destroy(gameObject, 60);
    }


    void Update()
    {
        
        if(hasRisen == false)
        {
            RiseCloud();

        }
        else
        {
            
            transform.RotateAround(planet.transform.position, rotationAxis, rotationSpeed * Time.deltaTime);
        }

        //rotatingAround();
    }

    private void FixedUpdate()
    {
        // Enable the rotating script after the object has risen past the threshold
        //if (!enabled && transform.position.magnitude > riseThreshold)
        //{
        //    enabled = true;
        //}

    }


    void RotatingAroundPlanetVanha()
    {
        // new vector3 voisi sisältää hiljalleen muuttuvia lukuja. Random 0-1 välillä tai että muuttuvat 0->1 erilaisilla nopeuksilla
        transform.RotateAround(pivotPoint.transform.position, new Vector3(0, 1, 1), rotationSpeed * Time.deltaTime);
    }

    void RotateAroundPlanetS(GameObject obj)
    {
        if (canRotate)
        {
            Vector3 pivotPoint = planet.transform.position; // Center of planet1

            // Calculate the direction vector from the object to the pivot point
            Vector3 toPivotDirection = pivotPoint - obj.transform.position;

            // Calculate the rotation axis perpendicular to the direction vector
            Vector3 rotationAxis = Vector3.Cross(obj.transform.up, toPivotDirection);

            // Rotate the object around the pivot point
            obj.transform.RotateAround(pivotPoint, rotationAxis, rotationSpeed * Time.deltaTime);
        }
    }





    void rotatingAround()
    {
        //if (planet != null)
        //{
        //    // Calculate rotation axis
        //    Vector3 toPlanetDirection = planet.position - transform.position;
        //    Vector3 rotationAxis = Vector3.Cross(transform.up, toPlanetDirection);

        //    // Rotate around the planet
        //    transform.RotateAround(planet.position, rotationAxis, rotationSpeed * Time.deltaTime);
        //}
        print("start rotating");
        rotationAxis = Vector3.Cross(riseDirection, transform.position - planet.transform.position).normalized;

    }

    public void RiseCloud()
    {
        // Move the cloud upward
        transform.Translate(riseDirection * riseSpeed * Time.deltaTime);

        // Check if the cloud has reached the maximum height
        if (transform.position.magnitude >= maxRiseHeight)
        {
            // Destroy the cloud when it reaches the maximum height
            //Destroy(gameObject);
            print("risen");

            hasRisen = true;
            //rotatingAround();
            //CalculateRotationAxis();
        }
    }

    void CalculateRotationAxis()
    {
        // Calculate rotation axis perpendicular to the rise direction and the vector from the cloud to the planet
        rotationAxis = Vector3.Cross(riseDirection, transform.position - planet.transform.position).normalized;
    }

    // Public method to set the rotation axis externally
    public void SetRotationAxis(Vector3 newRotationAxis)
    {
        rotationAxis = newRotationAxis.normalized;
    }

}
