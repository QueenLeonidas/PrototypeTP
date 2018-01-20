using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : SubscribedBehaviour {

    #region Variable Declarations
    [Header("Level Parameter")]
    public int levelNumber;

    [Header("Can't touch this")]
    public Transform buddySpawnPoint;
    public Transform playerTeleportPosition;
    public int orbsInLevel;
    #endregion



    #region Unity Event Functions
    // Use this for initialization
    void Start () {
        orbsInLevel = transform.findComponentsInChildrenWithTag<Transform>(Constants.TAG_ENERGY_ORB).Length;
        buddySpawnPoint = transform.findComponentInChildrenWithTag<Transform>(Constants.TAG_BUDDYSPAWN);
        playerTeleportPosition = transform.findComponentInChildrenWithTag<Transform>(Constants.TAG_PLAYERTELEPORT);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    #endregion



    #region Public Functions
    
    #endregion
}
