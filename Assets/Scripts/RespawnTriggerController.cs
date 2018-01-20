using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTriggerController : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if(other.tag == Constants.TAG_BUDDY) {
            other.GetComponent<BuddyController>().respawnBudddy(true);
        }
        if(other.tag == Constants.TAG_TELEKINETIC_OBJECT) {
            other.transform.parent.GetComponent<TelekineticObject>().respawnTelekineticObject();
        }
    }

}