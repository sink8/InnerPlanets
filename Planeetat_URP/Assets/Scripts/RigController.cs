using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigController : Movement
{
    Rigidbody rig;

    void OnEnable()
    {
        rig = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rig.AddForce((movementVector + gravityDirection * gravityStrength + jumpVector) * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}
