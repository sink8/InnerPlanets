using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Oma_playerController : MonoBehaviour
{
    //public InputAction playerControls;
    public PlayerControls _inputActions;
    CharacterController cc;

    Vector2 moveDirection = Vector2.zero;
    public float moveSpeed = 5f; // Movement speed
    public float sensitivity = 2f; // Mouse sensitivity

    private Transform playerTransform;

    Vector3 movementVector;


    private void OnEnable()
    {
        _inputActions = new PlayerControls();
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        //_inputActions.Disable();
    }
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move(_inputActions.PlayerActionmap.Moveing.ReadValue<Vector2>());  
        //Move(playerControls.PlayerActionmap.Move.ReadValue<Vector2>());
    }

    private void FixedUpdate()
    {
        cc.Move(movementVector * Time.deltaTime);
    }

    void Move(Vector2 _input)
    {
        //Debug.Log(_input.ToString());
        movementVector = transform.forward * _input.y + transform.right * _input.x;
    }


}
