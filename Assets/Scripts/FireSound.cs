using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class FireSound : SubscribedBehaviour {
	
	#region Variable Declarations
	#endregion
	
	
	
	#region Unity Event Functions
	private void Start() {
        StartCoroutine(waitAndPlay(GetComponent<ParticleSystem>().main.startDelay.constant));
	}
	
	private void Update() {
		
	}
    #endregion



    #region Private Functions
    IEnumerator waitAndPlay(float time) {
        yield return new WaitForSecondsRealtime(time);
        GetComponent<AudioSource>().Play();
    }
	#endregion
}
