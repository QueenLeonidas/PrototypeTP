using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Passenger : SubscribedBehaviour {

    #region Variable Declarations
    #endregion



    #region Unity Event Functions
    private void OnTriggerStay(Collider other) {
        if (other.tag.Contains(Constants.TAG_BUDDY)) {
            Vector3 newPosition = other.transform.position;
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            other.transform.position = newPosition;
        }
    }
    #endregion



    #region Private Functions
    #endregion
}
