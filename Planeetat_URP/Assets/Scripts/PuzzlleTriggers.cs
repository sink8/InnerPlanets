using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlleTriggers : MonoBehaviour
{

    public MouseLook MouseLook;
    public RotateAroundPlanet rotateAroundPlanet;

    public bool cloudsTrigger1 = false;
    public bool cloudsTrigger2 = false;

    public Animator animatorCloudsPuzzleRed;
    public Animator animatorCloudsPuzzleyellow;
    public bool cloudsActivated = false;
    public bool cloudsActivated1 = false;
    public bool cloudsActivated2 = false;
    public bool cloudsActivatedfinal = false;

    public Vector3 cloudsRotationAxis = new Vector3(1f, 1, 0);

    public GameObject windChanger;

    public GameObject redCube;
    public GameObject blueCube;

    public AudioSource audio1;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }


        if (!cloudsActivated) {
            ActivateCloudDirectionChange1();
            ActivateCloudDirectionChange2();
        }
        
        if(cloudsActivated && cloudsActivatedfinal == false)
        {
            ActivateCloudDirectionChange();
            StartCoroutine(enableWindChange());
            cloudsActivatedfinal = true;
        }

        if(cloudsActivated1 == true && cloudsActivated2 == true)
        {
            cloudsActivated = true;
        }
    }

    void ActivateCloudDirectionChange()
    {

            cloudsRotationAxis = new Vector3(0.3f, 1, 0);

 
    }
    void ActivateCloudDirectionChange1()
    {
        if (cloudsTrigger1 == true)
        {
            
            redCube.SetActive(false);

            animatorCloudsPuzzleRed.Play("purpleanim");

            
            cloudsActivated1 = true;
            MouseLook.pickable1_found = false;
        }
    }

    void ActivateCloudDirectionChange2()
    {
        if (cloudsTrigger2 == true)
        {
            
            blueCube.SetActive(false);
            animatorCloudsPuzzleyellow.Play("sinanim");

            
            cloudsActivated2 = true;
            MouseLook.pickable2_found = false;
        }
    }

    IEnumerator enableWindChange()
    {
        windChanger.SetActive(true);
        yield return new WaitForSeconds(3);
        animatorCloudsPuzzleRed.enabled = false;
        animatorCloudsPuzzleyellow.enabled = false;
        audio1.enabled = true;
        windChanger.SetActive(false);
    }
}
