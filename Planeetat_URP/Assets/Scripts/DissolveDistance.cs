using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;


public class DissolveDistance : MonoBehaviour
{
    public GameObject objectToTrack = null;

    Material materiala;
    Material dissolveMat;
    Renderer renderera;


    float fade = 0f;
    public bool isDissolving = false;
    bool broke = false;
    private void Awake()
    {
        materiala = this.GetComponent<Material>();
        dissolveMat = GetComponent<MeshRenderer>().material;
        renderera = this.GetComponent<Renderer>();
    }


    public Renderer Renderer{
        get
        {
            if (renderera == null)
                renderera = this.GetComponent<Renderer>();

            return renderera;
        }
    }
    public Material materialRef
    {
        get
        {
            if (materiala == null)
                materiala = Renderer.material;

            return materiala;
        }
    }

    private void Update()
    {
        //if (objectToTrack != null)
        //{
        //    Debug.Log(objectToTrack.transform.position);
        //    materialRef.SetVector("_Position", objectToTrack.transform.position);
        //}

        if (isDissolving == true)
        {
            DissolveFunctio();
        }
    }

    private void OnDestroy()
    {
        renderera = null;
        if(materiala != null)
        {
            Destroy(materiala);
        }

        materiala = null;
    }

    void DissolveFunctio()
    {
        if(broke == false) {
            fade += Time.deltaTime * 0.3f;

            if (fade >= 1f)
            {

                fade = 1f;
                //isDissolving = false;
                //Destroy(this.gameObject);
                //fireManager.Ashes.Remove(gameObject);
                broke = true;
            }

            dissolveMat.SetFloat("_Distance", fade);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Break"))
        {
            print(" collision happened");
            isDissolving = true;
            //Destroy(gameObject, 2f);

        }
    }

}
