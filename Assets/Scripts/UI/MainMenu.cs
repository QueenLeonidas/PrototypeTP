using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : SubscribedBehaviour {

    [SerializeField] float fadeTime = 2f;

    GameObject playButton,
               creditsButton,
               exitButton;
    HoloScreen holoScreen;



    // Use this for initialization
    void Start() {
        playButton = transform.Find("PlayButton").gameObject;
        creditsButton = transform.Find("CreditsButton").gameObject;
        exitButton = transform.Find("ExitButton").gameObject;
        holoScreen = transform.Find("HoloScreen").GetComponent<HoloScreen>();

        StartCoroutine(waitAndTurnOnButtonsCoroutine(2f));
    }

    // Update is called once per frame
    void Update() {

    }



    public void turnOffButtons() {
        playButton.GetComponent<BoxCollider>().enabled = false;
        LeanTween.alpha(playButton, 0f, fadeTime).setEase(LeanTweenType.easeInOutQuad);
        creditsButton.GetComponent<BoxCollider>().enabled = false;
        LeanTween.alpha(creditsButton, 0f, fadeTime).setEase(LeanTweenType.easeInOutQuad);
        exitButton.GetComponent<BoxCollider>().enabled = false;
        LeanTween.alpha(exitButton, 0f, fadeTime).setEase(LeanTweenType.easeInOutQuad);
    }

    public void showStory() {
        holoScreen.playVideo();
    }




    IEnumerator waitAndTurnOnButtonsCoroutine(float waitTime) {
        yield return new WaitForSecondsRealtime(waitTime);
        LeanTween.alpha(playButton, 1f, fadeTime).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => {
            playButton.GetComponent<BoxCollider>().enabled = true;
        });
        LeanTween.alpha(creditsButton, 1f, fadeTime).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => {
            creditsButton.GetComponent<BoxCollider>().enabled = true;
        });
        LeanTween.alpha(exitButton, 1f, fadeTime).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => {
            exitButton.GetComponent<BoxCollider>().enabled = true;
        });
    }
}