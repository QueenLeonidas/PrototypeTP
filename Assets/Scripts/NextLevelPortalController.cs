using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelPortalController : MonoBehaviour {

   

    private void OnTriggerStay(Collider other) {
        if(other.CompareTag(Constants.TAG_BUDDY)) {
            Vector3 buddyPosition = other.transform.position;
            buddyPosition.x = transform.position.x;
            buddyPosition.z = transform.position.z;
            other.transform.position = buddyPosition;
            
            other.GetComponent<BuddyController>().startLevelCompleteAnimation();
            GameEvents.StartLevelComplete();
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.CompareTag(Constants.TAG_BUDDY)) {
            other.GetComponent<BuddyController>().stopLevelCompleteAnimation();
        }
    }


}
