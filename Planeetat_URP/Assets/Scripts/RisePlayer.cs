using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisePlayer : MonoBehaviour
{
    public Vector3 targetPosition;
    public float riseSpeed = 1f;
    public float riseHeight = 2f;
    public float triggerDistance = 1f;

    public bool isRising = false;
    public GameObject player;
    public CharacterController characterController;
    public MoveData moveData;
    public bool forceChange = false;
    public bool touchingTrigger = false;

    public bool NormalGravity = false;
    public bool SpecialGravity = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //targetPosition = transform.position + Vector3.up * riseHeight;
            //isRising = true;
            if(SpecialGravity == true)
            {
                player = other.gameObject;
                moveData.jumpForce = 200f;
                forceChange = true;
            }

            if (SpecialGravity == false)
            {
                player = other.gameObject;
                moveData.jumpForce = 21f;
                forceChange = false;
            }
            //characterController.detectCollisions = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (SpecialGravity == true)
            {
                player = other.gameObject;
                moveData.jumpForce = 200f;
                forceChange = true;
            }

            if (SpecialGravity == false)
            {
                player = other.gameObject;
                moveData.jumpForce = 21f;
                forceChange = false;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //targetPosition = transform.position + Vector3.up * riseHeight;
            //isRising = true;
            //player = other.gameObject;
            //StartCoroutine(WaitWithExit());
            //return;

            //characterController.detectCollisions = false;
        }
    }

    IEnumerator WaitWithExit()
    {
        yield return new WaitForSeconds(3);
        moveData.jumpForce = 20f;
        forceChange = false;
    }

    // Update is called once per frame
    void Update()
    {


            if (isRising)
            {
                // Move the player upwards towards the target position
                player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, riseSpeed * Time.deltaTime);

                // If the player reaches the target position, stop rising
                if (player.transform.position == targetPosition)
                {
                    isRising = false;
                //characterController.detectCollisions = true;
            }
            }

    }

    void RisePlayerScript()
    {
        // Calculate the distance to the target position
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // If the distance is greater than triggerDistance, start rising
        if (distanceToTarget > triggerDistance)
        {
            // Move towards the target position
            player.transform.position = Vector3.MoveTowards(transform.position, targetPosition, riseSpeed * Time.deltaTime);
        }
        else
        {
            // Stop rising if the character has reached the target position
            isRising = false;
        }
    }
}

