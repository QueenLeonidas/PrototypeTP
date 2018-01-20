using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is last in the script execution order and can be used for calling functions at the end of the Start() Event.
/// </summary>
public class SceneInitializer : SubscribedBehaviour {

	// Use this for initialization
	void Start () {
        GameManager.Instance.respawnPlayer();
        GameManager.Instance.buddy.GetComponent<BuddyController>().respawnBudddy(false);
	}
}
