using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTrigger : MonoBehaviour
{
    public RotatePlanet planet;
    void Start()
    {
        
    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            print(" rotation trigger happened");
            planet.StartRotation();
            
        }



    }
}
