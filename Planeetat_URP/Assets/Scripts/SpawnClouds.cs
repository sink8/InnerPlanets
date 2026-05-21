using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnClouds : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 3;
    public GameObject pivotPoint;
    public Transform planet1; // Reference to the planet1 object
    public PuzzlleTriggers trigger;

    public GameObject objectToSpawn;
    public Transform spawnArea;
    public float spawnRate = 1f;
    public float spawnRadius = 5f;
    public float riseSpeed = 1f;
    public float riseThreshold = 10f; // Height at which rotation starts
    public bool canRotate = false;
    public float maxRiseHeight = 10f;

    private float lastSpawnTime;

    public Vector3 rotationAxis;
    void Start()
    {
     
        GameObject g = GameObject.Find("CloudsChanger");
        trigger = g.GetComponent<PuzzlleTriggers>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSpawnTime > spawnRate)
        {
            SpawnObject();
            lastSpawnTime = Time.time;
        }
    }

    void SpawnObject()
    {
        // Calculate spawn position within spawn radius

        rotationAxis = trigger.cloudsRotationAxis;
        Vector3 randomDirection = Random.onUnitSphere;
        Vector3 spawnPosition = spawnArea.position;

        // Instantiate cloud prefab at the spawn position
        GameObject newCloud = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

        // Attach the CloudRiser script to the newly spawned cloud
        RotateAroundPlanet cloudRiser = newCloud.GetComponent<RotateAroundPlanet>();
        cloudRiser.riseSpeed = riseSpeed;
        cloudRiser.maxRiseHeight = maxRiseHeight;

        if(cloudRiser != null)
        {
            cloudRiser.SetRotationAxis(rotationAxis);
        }
    }
}
