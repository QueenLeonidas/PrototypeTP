using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TeleDistance : TelekineticObject {

    #region Variable Declarations
    [Header("Distance Cube")]
    [Range(0, 50)]
    [SerializeField] float maxDistance = 6;
    [SerializeField] int lineRendererResolution = 16;
    [Range(0, 1)]
    [SerializeField] float emptyVolume = 1;
    [SerializeField] Gradient glowGradient;

    Vector3 cubePosition;
    LineRenderer lineRenderer;
    #endregion



    #region Unity Event Functions
    protected override void Start() {
        base.Start();

        lineRenderer = GetComponent<LineRenderer>();
    }

    protected override void Update() {
        base.Update();

        handleDistanceTransgression();
        updateGlowIntensity();
    }
    #endregion

    public override void startTelekineticMovement() {
        base.startTelekineticMovement();

        drawMovementRadius();
    }


    #region Public Functions
    public override void moveTelekineticObject(Vector3 targetPosition) {

        handleDistanceTransgression();

        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float moveDistance = (targetPosition - transform.position).magnitude;
        rigidbody.velocity = moveDirection * moveDistance * moveMultiplier;

        updateMovementRadius();

        base.moveTelekineticObject(targetPosition);
    }

    public override void stopTelekineticMovement() {
        TelekinesisController.Instance.telekinesisInProgress = false;
        rigidbody.velocity = Vector3.zero;

        lineRenderer.positionCount = 0;

        base.stopTelekineticMovement();
    }
    #endregion



    #region Private Functions
    private void handleDistanceTransgression() {
        Vector3 xzCubeToOrigin = getXZCubeToOrigin();
        if(xzCubeToOrigin.magnitude >= maxDistance ) {
                transform.position = new Vector3(base.startPosition.x, transform.position.y, base.startPosition.z) + xzCubeToOrigin.normalized * (maxDistance - 0.5f);
                stopTelekineticMovement();
                playCubeEmptyAudio();
        }
    }

    private Vector3 getXZCubeToOrigin() {
        Vector3 xzPosition = Vector3.Scale(transform.position, new Vector3(1, 0, 1));
        Vector3 xzStart = Vector3.Scale(startPosition, new Vector3(1, 0, 1));
        return xzPosition - xzStart;
    }

    private void playCubeEmptyAudio() {
        audioSource.PlayOneShot(AudioManager.Instance.GetAudioClip(Constants.SOUND_CUBE_EMPTY), emptyVolume);
    }

    private void updateMovementRadius() {
        // Update y position of lineRenderer
        for (int i = 0; i < lineRenderer.positionCount; i++) {
            lineRenderer.SetPosition(i, new Vector3(lineRenderer.GetPosition(i).x, transform.position.y, lineRenderer.GetPosition(i).z));
        }
    }

    private void drawMovementRadius() {
        lineRenderer.positionCount = lineRendererResolution;
        for (int i = 0; i < lineRendererResolution; ++i) {
            float factor = (i / (float)lineRendererResolution) * Mathf.PI * 2f;
            Vector3 pos = new Vector3(
                Mathf.Sin(factor) * maxDistance,
                transform.position.y - startPosition.y,
                Mathf.Cos(factor) * maxDistance);
            lineRenderer.SetPosition(i, pos + startPosition);
        }
    }

    private void updateGlowIntensity() {
        glowMat.SetColor("_EmissionColor", glowGradient.Evaluate(getXZCubeToOrigin().magnitude / maxDistance));
    }
    #endregion
}
