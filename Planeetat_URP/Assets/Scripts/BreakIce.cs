using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakIce : MonoBehaviour
{

    Collider collider1;

    public bool iceBroke = false;

    void Start()
    {
        collider1 = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Break"))
        {
            print(" collision happened");
            collider1.enabled = false;
            iceBroke = true;
            //Destroy(gameObject,2f);

        }
    }

}
