using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TeleFlying : TelekineticObject {

    #region Variable Declarations
	#endregion
	
	
	
	#region Unity Event Functions
    
    #endregion



    #region Public Functions
    public override void moveTelekineticObject(Vector3 targetPosition) {

        // Calculation for the rotation of the cube seems to be a bit complicated (Quaternions...)
        //transform.forward = Vector3.Scale(GameManager.Instance.player.transform.forward, new Vector3(1, 0, 1));
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float moveDistance = (targetPosition - transform.position).magnitude;
        rigidbody.velocity = moveDirection * moveDistance * moveMultiplier;

        base.moveTelekineticObject(targetPosition);
    }
    #endregion



    #region Private Functions
    #endregion
}
