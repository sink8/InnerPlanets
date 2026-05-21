using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopFire : MonoBehaviour
{
    public ParticleSystem _particleSystem;
    public Animator animatorJoki;
    public GameObject fireCollider;
    public Collider collider1;
    public float joki = 1;
    public bool jokiactivated = false;

    public BreakIce breakIce;



    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.CompareTag("Sade"))
        {
            print("sade osui");
            if(breakIce.iceBroke == true)
            {
                if(joki == 1)
                {
                    StartCoroutine(WaitForCollider());
                    animatorJoki.Play("Joki1Animaatio");
                    jokiactivated=true;
                }
                if (joki == 2)
                {
                    StartCoroutine(WaitForCollider());
                    animatorJoki.Play("Joki2Animaatio");
                }

            }
        }

    }

    IEnumerator WaitForCollider()
    {
        yield return new WaitForSeconds(3);
        collider1.enabled = false;
        _particleSystem. Stop(true);
    }
}
