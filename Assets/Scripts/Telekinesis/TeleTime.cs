using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TeleTime : TelekineticObject {

    #region Variable Declarations
    [Header("Time Cube")]
    [SerializeField] float moveTime = 1;
    [Range(0, 1)]
    [SerializeField]
    float emptyVolume = 1;

    Coroutine timerCoroutine;
    int tweenID;
    #endregion



    #region Unity Event Functions

    #endregion



    #region Public Functions
    public override void startTelekineticMovement() {
        base.startTelekineticMovement();

        timerCoroutine = StartCoroutine(cubeTimer());
        tweenID = LeanTween.value(gameObject, glowColor, Color.black, moveTime).setOnUpdate((Color val) => {
            glowMat.SetColor("_EmissionColor", val);
        }).id;
    }

    public override void moveTelekineticObject(Vector3 targetPosition) {
        
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float moveDistance = (targetPosition - transform.position).magnitude;
        rigidbody.velocity = moveDirection * moveDistance * moveMultiplier;

        base.moveTelekineticObject(targetPosition);
    }

    public override void stopTelekineticMovement() {
        TelekinesisController.Instance.telekinesisInProgress = false;
        rigidbody.velocity = Vector3.zero;
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        LeanTween.cancel(tweenID);
        tweenID = LeanTween.value(gameObject, glowMat.GetColor("_EmissionColor"), glowColor, moveTime).setOnUpdate((Color val) => {
            glowMat.SetColor("_EmissionColor", val);
        }).id;

        base.stopTelekineticMovement();
    }
    #endregion



    #region Private Functions
    IEnumerator cubeTimer() {
        yield return new WaitForSecondsRealtime(moveTime);
        stopTelekineticMovement();
        audioSource.PlayOneShot(AudioManager.Instance.GetAudioClip(Constants.SOUND_CUBE_EMPTY), emptyVolume);
    }
    #endregion
}
