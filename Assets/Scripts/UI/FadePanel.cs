using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class FadePanel : SubscribedBehaviour {

    #region Variable Declarations
    [SerializeField] float fadeSpeed = 1f;

    public static FadePanel Instance;
    #endregion



    #region Unity Event Functions
    //Awake is always called before any Start functions
    void Awake() {
        //Check if instance already exists
        if (Instance == null)

            //if not, set instance to this
            Instance = this;

        //If instance already exists and it's not this:
        else if (Instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }
    #endregion



    #region Public Functions
    public void fadeIn() {
        LeanTween.alpha(GetComponent<RectTransform>(), 0f, fadeSpeed).setEase(LeanTweenType.easeInOutQuad);
    }

    public void fadeIn(System.Action onComplete) {
        LeanTween.alpha(GetComponent<RectTransform>(), 0f, fadeSpeed).setEase(LeanTweenType.easeInOutQuad).setOnComplete(onComplete);
    }

    public void fadeOut() {
        LeanTween.alpha(GetComponent<RectTransform>(), 1f, fadeSpeed).setEase(LeanTweenType.easeInOutQuad);
    }

    public void fadeOut(System.Action onComplete) {
        LeanTween.alpha(GetComponent<RectTransform>(), 1f, fadeSpeed).setEase(LeanTweenType.easeInOutQuad).setOnComplete(onComplete);
    }
    #endregion
}
