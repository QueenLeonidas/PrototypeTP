using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles everything related to the movement of the Buddy
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class BuddyController : SubscribedBehaviour {

    #region Variable Declarations
    [Header("Sound Volumes")]
    [Range(0, 1)]
    [SerializeField] float deathVolume = 1;
    [Range(0, 1)]
    [SerializeField]
    float spawnVolume = 1;
    [Range(0, 1)]
    [SerializeField]
    float cameraEnterVolume = 1;
    [Range(0, 1)]
    [SerializeField]
    float cameraExitVolume = 1;
    [Range(0, 1)]
    [SerializeField]
    float orbPickupVolume = 1;

    private bool wasInView = false;
    private bool buddyRespawning = false;

    // Component References
    private new Rigidbody rigidbody;
    private Animator animator;
    private AudioSource audioSource;
    #endregion



    #region Unity Event Functions
    private void Start() {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if (Input.GetButtonDown(Constants.INPUT_BUDDY_RESPAWN)) {
            respawnBudddy(false);
        }
    }

    private void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.CompareTag(Constants.TAG_ENERGY_ORB)) {
            collectEnergyOrb(collision.gameObject);
            audioSource.PlayOneShot(AudioManager.Instance.GetAudioClip(Constants.SOUND_ORB_PICKUP), orbPickupVolume);
        }
    }
    #endregion



    #region public Functions
    public void respawnBudddy(bool died) {
        StartCoroutine(respawnBuddyCoroutine(died));
    }
    public void startLevelCompleteAnimation() {
        animator.SetBool("LevelComplete",true);
    }
    public void stopLevelCompleteAnimation() {
        animator.SetBool("LevelComplete", false);
    }
    public void setGameEndAnimation() {
        animator.SetBool("GameEnd", true);
    }
    public void resetEndLevelAnimation() {
        animator.SetBool("GameEnd", false);
    }
   
    #endregion



    #region Custom Event Functions
    protected override void OnNextLevel() {
        respawnBudddy(false);
    }
    #endregion



    #region Private Functions
    private void collectEnergyOrb(GameObject energyOrb) {
        Destroy(energyOrb);
        GameManager.Instance.increaseBuddyEnergyLevel(1);
        GameManager.Instance.setLastEnergyOrbPosition(energyOrb.transform.position);
        animator.SetTrigger("Collect");
        audioSource.PlayOneShot(AudioManager.Instance.GetAudioClip(Constants.SOUND_BUDDY_HAPPY));
    }

   
    // Decided: This won't be in the game (too annoying. happens too often. 
    // Buddy's movement shall only be blocked when out of screen)
    //private void checkIfBuddyOnScreen() {
    //    // Check only if buddy isn't falling
    //    if (rigidbody.velocity.y < 0) return;

    //    if (!wasInView && gameObject.objectInView()) {
    //        audioSource.PlayOneShot(AudioManager.Instance.GetAudioClip(Constants.SOUND_BUDDY_CAMERA_ENTER), cameraEnterVolume);
    //        wasInView = true;
    //    }
    //    else if (wasInView && !gameObject.objectInView()) {
    //        audioSource.PlayOneShot(AudioManager.Instance.GetAudioClip(Constants.SOUND_BUDDY_CAMERA_EXIT), cameraExitVolume);
    //        wasInView = false;
    //    }
    //}
    #endregion



    #region Coroutines
    IEnumerator respawnBuddyCoroutine(bool died) {
        // Exit if already respawning
        if (buddyRespawning || GameManager.Instance.getCurrentLevel() == 0) yield break;

        buddyRespawning = true;
        if (died) {
            audioSource.PlayOneShot(AudioManager.Instance.GetAudioClip(Constants.SOUND_BUDDY_DEATH), deathVolume);
            yield return new WaitForSecondsRealtime(2.5f);
        }

        rigidbody.velocity = Vector3.zero;
        gameObject.transform.position = GameManager.Instance.getCurrentBuddySpawn().position + Vector3.up * 1f;
        transform.LookAt( new Vector3(GameManager.Instance.player.transform.position.x, transform.position.y, GameManager.Instance.player.transform.position.z), Vector3.up);

        GameManager.Instance.getCurrentBuddySpawn().Find("Buddy Spawn PARTCL").GetComponent<ParticleSystem>().Play();
        audioSource.PlayOneShot(AudioManager.Instance.GetAudioClip(Constants.SOUND_BUDDY_SPAWN), spawnVolume);
        animator.SetTrigger("Spawn");
        yield return new WaitForSecondsRealtime(1f);
        buddyRespawning = false;
    }

    
    #endregion
}
