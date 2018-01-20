using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class HoloBuddyLogo : SubscribedBehaviour {

    #region Variable Declarations
	#endregion
	
	
	
	#region Unity Event Functions
	private void Start () {
        LeanTween.alpha(GetComponent<RectTransform>(), 1f, 2f).setDelay(7f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => {
            LeanTween.alpha(GetComponent<RectTransform>(), 0f, 2f).setDelay(1.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => {
                LoadNextScene();
            });
        });
	}
    #endregion



    #region Private Functions
    private void LoadNextScene() {
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        if (activeScene + 1 < SceneManager.sceneCountInBuildSettings) {
            SceneManager.LoadScene(activeScene + 1);
        }
        else {
            Debug.LogError("No more levels in build index to be loaded.");
        }
    }
    #endregion
}
