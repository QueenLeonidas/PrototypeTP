using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put this on a Level GameObject to play all HoloScreens in the level when the player reaches this level
/// </summary>
public class PlayAllHoloScreensInLevel : SubscribedBehaviour {

    [SerializeField] float holoScreenActivationOffset = 0.1f;

    HoloScreen[] holoScreens;
    Level levelScript;

    // Use this for initialization
    void Start () {
        holoScreens = gameObject.GetComponentsInChildren<HoloScreen>();
        levelScript = GetComponent<Level>();
    }



    protected override void OnNextLevel() {
        if (GameManager.Instance.getCurrentLevel() == levelScript.levelNumber) {
            for (int i = 0; i < holoScreens.Length; i++) {
                StartCoroutine(waitAndActivateHoloScreen(holoScreens[i], i * holoScreenActivationOffset));
            }
        }
    }



    IEnumerator waitAndActivateHoloScreen(HoloScreen holoScreen, float waitTime) {
        yield return new WaitForSecondsRealtime(waitTime);
        holoScreen.playVideo();
    }
}
