using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MouseLook : MonoBehaviour
{
    #region variables

    public UnityEvent OnClick;
    public UnityEvent OnLook;
    public UnityEvent OnUnLook;


    Vector2 _mouseFinal;
    Vector2 _smoothMouse;

    public Vector2 clampInDegrees = new Vector2(360, 180);
    public bool lockCursor;

    public Vector2 sensitivity = new Vector2(2, 2);
    public Vector2 smoothing = new Vector2(3, 3);
    Vector2 targetDirection;
    Vector2 targetCharacterDirection;

    public GameObject characterBody;

    public PlayerControls _input;
    public PuzzlleTriggers _triggers;
    public StopFire stopFire1;
    public UIScripts _uiscripts;

    public GameObject activat;
    public GameObject cantActiv;
    public GameObject fireHot;
    public GameObject Icebreak;
    public GameObject pushIt;
    public GameObject dryRiver;
    public GameObject windDirection;
    public GameObject redCube;
    public GameObject blueCube;
    public GameObject redCube1;
    public GameObject blueCube1;
    public GameObject valkoinen;
    public GameObject valko_koskettu;

    public Camera _camera;


    public bool pickable1_found = false;
    public bool pickable2_found = false;
    public bool touched = false;

    public ParticleSystem tunneli;
    public GameObject tunneliPrefab;
    #endregion

    private void OnEnable()
    {
        _input = new PlayerControls();
        _input.Enable();
    }

    void Start()
    {
        // Set target direction to the camera's initial orientation.
        targetDirection = transform.localRotation.eulerAngles;

        // Set target direction for the character body to its inital state.
        if (characterBody)
            targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;

    }

    Vector2 ScaleAndSmooth(Vector2 _delta)
    {
        //Apply sensetivity
        _delta = Vector2.Scale(_delta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        //Lerp from last frame
        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, _delta.x, 1f / smoothing.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, _delta.y, 1f / smoothing.y);

        return _smoothMouse;
    }

    void LateUpdate()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        Vector2 mouseDelta = _input.PlayerActionmap.MouseLook.ReadValue<Vector2>();
        _mouseFinal += ScaleAndSmooth(mouseDelta);

        ClampValues();
        AlignToBody();

        CastRay();
        HitMouseButton();

        if (pickable1_found == true)
        {
            redCube.SetActive(true);
        } else
        {
            redCube.SetActive(false);
        }
        if (pickable2_found == true)
        {
            blueCube.SetActive(true);
        }
        else
        {
            blueCube.SetActive(false);
        }

    }

    void ClampValues()
    {
        // Clamp and apply the local x value first
        if (clampInDegrees.x < 360)
            _mouseFinal.x = Mathf.Clamp(_mouseFinal.x, -clampInDegrees.x * 0.4f, clampInDegrees.x * 0.4f);

        // Then clamp y value.
        if (clampInDegrees.y < 360)
            _mouseFinal.y = Mathf.Clamp(_mouseFinal.y, -clampInDegrees.y * 0.4f, clampInDegrees.y * 0.4f);

        // Allow the script to clamp based on a desired target value.
        var targetOrientation = Quaternion.Euler(targetDirection);
        transform.localRotation = Quaternion.AngleAxis(-_mouseFinal.y, targetOrientation * Vector3.right) * targetOrientation;

    }

    void AlignToBody()
    {
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

        // If there's a character body that acts as a parent to the camera
        if (characterBody)
        {
            var yRotation = Quaternion.AngleAxis(_mouseFinal.x, Vector3.up);
            characterBody.transform.localRotation = yRotation * targetCharacterOrientation;
        }
        else
        {
            var yRotation = Quaternion.AngleAxis(_mouseFinal.x, transform.InverseTransformDirection(Vector3.up));
            transform.localRotation *= yRotation;
        }
    }

    void CastRay()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 25f;
        mousePos = _camera.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(transform.position, mousePos - transform.position, Color.blue);

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 25))
        {
            
            if (hit.transform.name == "rauniot" || hit.transform.name == "object2")
            {
                _uiscripts.EnableText();
            }

            else if ((hit.transform.name == "raunio11" && pickable1_found == true) || (hit.transform.name == "raunio22" && pickable2_found == true))
            {
                activat.SetActive(true);
            }
            //else if ((hit.transform.name == "CloudsChanger1" && pickable1_found == false) || (hit.transform.name == "CloudsChanger2" && pickable2_found == false))
            //{
            //    cantActiv.SetActive(true);
            //}
            else if ((hit.transform.name == "lava") || (hit.transform.name == "TuliCollider1") || (hit.transform.name == "TuliCollider2") && stopFire1.jokiactivated == true)
            {
                fireHot.SetActive(true);
            }
            else if (hit.transform.name == "Joki1" && stopFire1.jokiactivated == false )
            {
                dryRiver.SetActive(true);
            }
            else if (hit.transform.name == "työnnä")
            {
                pushIt.SetActive(true);
            }
            else if (hit.transform.name == "kristalli_valko" && touched == false)
            {
                valkoinen.SetActive(true);
            }
            else if (hit.transform.name == "kristalli_valko" && touched == true)
            {
                valko_koskettu.SetActive(true);
            }
            else if (hit.transform.name == "Wind" && pickable1_found == true)
            {
                //dryRiver.SetActive(true);
            }

            else 
            {
                activat.SetActive(false);
                cantActiv.SetActive(false);
                fireHot.SetActive(false);
                _uiscripts.DisableText();
                dryRiver.SetActive(false);
                pushIt.SetActive(false);
                valko_koskettu.SetActive(false);
                valkoinen.SetActive(false);
            }

        }
    }

    public void HitMouseButton()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 50))
            {
                Debug.Log(hit.transform.name);

                if(hit.transform.name == "rauniot")
                {
                    pickable1_found = true;
                    redCube1.SetActive(false);
                }
                if (hit.transform.name == "object2")
                {
                    pickable2_found = true;
                    blueCube1.SetActive(false);
                }
                if (hit.transform.name == "kristalli_valko")
                {
                    touched = true;
                    tunneli.Play();
                    tunneliPrefab.SetActive(true);
                }
                if (hit.transform.name == "raunio11" && pickable1_found == true)
                {
                    _triggers.cloudsTrigger1 = true;
                }
                if (hit.transform.name == "raunio22" && pickable2_found == true)
                {
                    _triggers.cloudsTrigger2 = true;
                }

            }
        }
    }
}
