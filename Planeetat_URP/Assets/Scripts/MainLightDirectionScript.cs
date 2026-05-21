using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLightDirectionScript : MonoBehaviour
{

    [SerializeField] Material skyboxMaterial;

    private void Update()
    {
        skyboxMaterial.SetVector("_MainLightDirection",transform.forward);
    }

}
