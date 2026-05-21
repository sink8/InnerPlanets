using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlkuKoodit : MonoBehaviour
{
    public Camera cam;
    public GameObject cam2;
    public GameObject cam1;
    public Animator animator;
    public Animator animatorBlack;
    public Vector3 alkunCameraPos;
    public Vector3 alkunCameraRot;

    public GameObject shipUI;
    public GameObject dollytack;
    public GameObject VirtualCamera;
    public GameObject ship;
    public GameObject light1;
    public GameObject musiikki;


    void Start()
    {
        alkunCameraPos = new Vector3(0,1,0);
        alkunCameraRot = new Vector3(0,0,0);

        PlayCameraAnimation();
        StartCoroutine(ExampleCoroutinedark());
        StartCoroutine(ExampleCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayCameraAnimation()
    {
        if (animator == null) { return; }

        animator.Play("cameraanim");
    }

    void PlayCameraAnimationBlack()
    {
        if (animator == null) { return; }

        animatorBlack.Play("blackui");
    }

    void FadeBlack()
    {

    }

    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(16f);
        
        cam1.SetActive(true);
        cam2.SetActive(false);
        shipUI.SetActive(false);
        dollytack.SetActive(false);
        VirtualCamera.SetActive(false);
        ship.SetActive(true);
        light1.SetActive(true);
        musiikki.SetActive(true);
        cam.transform.position = alkunCameraPos;
        cam.transform.rotation = Quaternion.identity;
 

    }

    IEnumerator ExampleCoroutinedark()
    {
        yield return new WaitForSeconds(12.0f);
        PlayCameraAnimationBlack();



    }
}
