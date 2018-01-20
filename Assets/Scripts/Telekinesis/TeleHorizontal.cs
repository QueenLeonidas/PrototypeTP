using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TeleHorizontal : TelekineticObject {

    #region Variable Declarations
    [Header("Horizontal Cube")]
    [SerializeField] bool moveOnZAxis = false;
    #endregion



    #region Unity Event Functions

    #endregion



    #region Public Functions
    public override void startTelekineticMovement() {
        base.startTelekineticMovement();

        transform.position += new Vector3(0, 0.05f, 0);
        rigidbody.constraints = rigidbody.constraints | RigidbodyConstraints.FreezePositionY;
    }

    public override void moveTelekineticObject(Vector3 targetPosition) {

        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float moveDistance = (targetPosition - transform.position).magnitude;
        if (!moveOnZAxis) {
            rigidbody.velocity = Vector3.Scale(moveDirection * moveDistance * moveMultiplier, new Vector3(1, 0, 0));
        } else {
            rigidbody.velocity = Vector3.Scale(moveDirection * moveDistance * moveMultiplier, new Vector3(0, 0, 1));
        }

        base.moveTelekineticObject(targetPosition);
    }

    public override void stopTelekineticMovement() {
        base.stopTelekineticMovement();

        rigidbody.constraints = rigidbody.constraints & ~RigidbodyConstraints.FreezePositionY;
    }
    #endregion



    #region Private Functions
    #endregion
}
