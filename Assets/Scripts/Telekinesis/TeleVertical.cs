using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TeleVertical : TelekineticObject {

    #region Variable Declarations
	#endregion
	
	
	
	#region Unity Event Functions
    
    #endregion



    #region Public Functions
    public override void moveTelekineticObject(Vector3 targetPosition) {
        
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float moveDistance = (targetPosition - transform.position).magnitude;
        rigidbody.velocity = Vector3.Scale(moveDirection * moveDistance * moveMultiplier, new Vector3(0, 1, 0));

        base.moveTelekineticObject(targetPosition);
    }
    #endregion



    #region Private Functions
    #endregion
}
