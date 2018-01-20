using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
abstract public class TelekineticObject : SubscribedBehaviour {

    #region Variable Declarations
    [Header("Movement")]
    [SerializeField] protected float moveMultiplier = 3;
    [SerializeField] protected float maxMoveSpeed = 4;
    [SerializeField] protected float telekinizableCooldown = 1f;

    [Header("Sound Volumes")]
    [Range(0, 1)]
    [SerializeField]
    float pickupVolume = 1;
    [Range(0, 1)]
    [SerializeField]
    float floatVolume = 1;

    protected Vector3 startPosition;
    protected Quaternion startRotation;
    protected Vector3 targetPosition;
    [SerializeField] protected bool telekinizable = true;
    protected bool checkForTelekinizeReset = false;

    protected new Rigidbody rigidbody;
    protected AudioSource audioSource;
    protected GameObject passengerTrigger;
    protected Material glowMat;
    protected Color glowColor;

    public bool Telekinizable { get { return telekinizable; } }
    #endregion



    #region Unity Event Functions
    protected virtual void Start() {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        glowMat = transform.GetChild(0).GetComponent<Renderer>().materials[1];
        glowColor = glowMat.GetColor("_EmissionColor");

        passengerTrigger = transform.Find("PassengerTrigger").gameObject;
        passengerTrigger.SetActive(false);


        startPosition = transform.position;
        startRotation = transform.rotation;
	}

    protected virtual void Update() {
        if (checkForTelekinizeReset && rigidbody.velocity.magnitude <= 0.001f) {
            telekinizable = true;
            checkForTelekinizeReset = false;
        }
    }

    protected virtual void OnDrawGizmosSelected() {
        if (targetPosition != Vector3.zero) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(targetPosition, 1f);
        }
    }
    #endregion



    #region Public Functions
    public void respawnTelekineticObject() {
        transform.position = startPosition;
        transform.rotation = startRotation;
        rigidbody.velocity = Vector3.zero;
        rigidbody.useGravity = true;
    }

    virtual public void startTelekineticMovement() {
        passengerTrigger.SetActive(true);
        rigidbody.useGravity = false;
        audioSource.PlayOneShot(AudioManager.Instance.GetAudioClip(Constants.SOUND_CUBE_PICKUP), pickupVolume);
        AudioManager.Instance.PlayAudio(Constants.SOUND_CUBE_FLOAT, floatVolume);
    }

    virtual public void moveTelekineticObject(Vector3 targetPosition) {
        this.targetPosition = targetPosition;
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxMoveSpeed);
    }

    virtual public void stopTelekineticMovement() {
        passengerTrigger.SetActive(false);
        this.targetPosition = Vector3.zero;
        rigidbody.useGravity = true;
        telekinizable = false;
        StartCoroutine(telekinizableCooldownCoroutine());
        AudioManager.Instance.StopAudio(Constants.SOUND_CUBE_FLOAT);
    }
    #endregion



    #region Private Functions
    IEnumerator telekinizableCooldownCoroutine() {
        yield return new WaitForSecondsRealtime(telekinizableCooldown);
        checkForTelekinizeReset = true;
    }
    #endregion
}
