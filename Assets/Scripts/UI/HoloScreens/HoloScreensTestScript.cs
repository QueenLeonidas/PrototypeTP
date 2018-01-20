using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class HoloScreensTestScript : SubscribedBehaviour {

    #region Variable Declarations
    HoloScreen[] screensToStart;
	#endregion
	
	
	
	#region Unity Event Functions
	private void Start () {
        screensToStart = GameObject.FindObjectsOfType<HoloScreen>();

        StartCoroutine(waitAndStartVideos());
        

        StartCoroutine(waitAndStopVideos());
	}
	
	private void Update () {
		
	}
    #endregion



    IEnumerator waitAndStopVideos() {
        yield return new WaitForSecondsRealtime(5f);
        print("Stopping videos");
        foreach (HoloScreen screen in screensToStart) {
            screen.stopCurrentVideo();
        }
    }

    IEnumerator waitAndStartVideos() {
        yield return new WaitForSecondsRealtime(1f);
        print("Starting videos");
        foreach (HoloScreen screen in screensToStart) {
            screen.playVideo();
        }
    }
}
