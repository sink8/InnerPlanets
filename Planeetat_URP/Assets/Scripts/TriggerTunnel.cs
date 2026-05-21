using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTunnel : MonoBehaviour
{

    public ParticleSystem tunneli;
    public GameObject tunneliPrefab;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Tunnel"))
        {
            Debug.Log("Triggered by tunnel");
        }

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("player");
            tunneli.Play();
            tunneliPrefab.SetActive(true);
        }



    }
}
