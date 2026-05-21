using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    #region --
    PlayerControls _inputActions;

    [SerializeField]
    protected MoveData moveData;

    public Transform planet;
    [SerializeField]
    Transform transformBody;

    protected Vector3 movementVector;
    protected Vector3 gravityDirection;
    protected float gravityStrength = 1f;
    protected Vector3 jumpVector;

    [SerializeField]
    LayerMask GroundLayerMask;
    [SerializeField]
    Transform groundCollider;

    RayData groundData;
    public RisePlayer risePlayer;

    public float raySize = 5f;
    public bool runIsPressed = false;
    public bool isGrounded = false;
    public bool startedJUmping = false;
    public float jumpForcedefault = 0f;

    public float airMoveSpeed = 3f; // The speed of movement in the air
    public float airMoveDuration = 1f; // The duration of air movement
    public Vector3 airMoveDirection = Vector3.right; // The direction of air movement

    [SerializeField]
    float pushForce;
    public CharacterController cc;
    #endregion

    void Start()
    {
        groundData = new RayData();
        isGrounded = groundData.grounded;

        _inputActions = new PlayerControls();
        _inputActions.Enable();
        _inputActions.PlayerActionmap.Jump.performed += Jump;
        jumpForcedefault = moveData.jumpForce;
    }

    void Update()
    {
        ApplyGravity();
        CheckGround();
        RotateToSurface();
        Move();
    }

    void ApplyGravity()
    {
        gravityDirection = (planet.position - transform.position).normalized;

        if (!groundData.grounded)
            gravityStrength += planet.GetComponent<Planet>().GravitationalPull * Time.deltaTime;
        else
            gravityStrength = moveData.surfaceGravity;
    }

    void RotateToSurface()
    {
        Quaternion gravityRotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;
        Quaternion surfaceRotation = Quaternion.FromToRotation(transform.up, groundData.normal) * transform.rotation;
        Quaternion finalRotation = Quaternion.Lerp(gravityRotation, surfaceRotation, moveData.stickToSurface);

        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, moveData.surfaceRotationSpeed * Time.deltaTime);
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (groundData.grounded)
            StartCoroutine(ApplyJump());
    }

    IEnumerator ApplyJump()
    {
        gravityStrength = 0f;
        jumpVector = Vector3.zero;
        startedJUmping = true;
        float force = moveData.jumpForce;
        float t = 0f;

        while (t < moveData.jumpDuration)
        {
            jumpVector = -gravityDirection * force;
            force = Mathf.Lerp(moveData.jumpForce, 0f, t / moveData.jumpDuration);
            t += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        jumpVector = Vector3.zero;
        moveData.jumpForce = jumpForcedefault;
    }


    IEnumerator ApplyAirMovement()
    {
        float t = 0f;

        // Move in air for a certain duration
        while (t < moveData.airMoveDuration)
        {
            // Calculate movement vector based on input
            Vector2 input = _inputActions.PlayerActionmap.Moveing.ReadValue<Vector2>();
            Vector3 airMovementVector = (transformBody.forward * input.y + transformBody.right * input.x) * moveData.airMoveSpeed;

            // Apply movement
            //characterController.Move(airMovementVector * Time.deltaTime);

            t += Time.deltaTime;
            yield return null;
        }
    }

    void Move()
    {
        Vector2 input = _inputActions.PlayerActionmap.Moveing.ReadValue<Vector2>();
        //runIsPressed = _inputActions.PlayerActionmap.Run.ReadValue <bool>(); //ei toimi

        //if (groundData.grounded)
            //{

            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementVector = (transformBody.forward * input.y + transformBody.right * input.x) * moveData.moveSpeed * 2;
                print("running");
            }
            else
            {
                movementVector = (transformBody.forward * input.y + transformBody.right * input.x) * moveData.moveSpeed;
            }
        //}
    }

    void CheckGround()
    {
        if (Physics.CheckSphere(groundCollider.position, moveData.groundColSize, GroundLayerMask))
        {
            RaycastHit hit;
            Physics.Raycast(groundCollider.position, -transform.up, out hit, raySize);
            groundData.grounded = true;

            if(risePlayer.forceChange == false)
            {
                moveData.jumpForce = jumpForcedefault;
            }
            
            //print("osui");
            groundData.normal = hit.normal;
            return;
        }

        groundData.grounded = false;
        startedJUmping = false;
        
        groundData.normal = -gravityDirection;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!cc.isGrounded && hit.transform.tag == "Break")
        {
            print("hit break)");
        }
        Rigidbody rb = hit.collider.attachedRigidbody;
        if(rb != null && !rb.isKinematic)
        {
            rb.velocity = hit.moveDirection * pushForce;
        }
        
    }


}
