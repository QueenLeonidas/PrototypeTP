using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// This class gives the player the possibility to select UI Entries in the world by centering the view on the object for x seconds.
/// </summary>
public class PlayerSelect : SubscribedBehaviour {

    public static PlayerSelect Instance;
    
    [SerializeField] float timeToSelect = 3;

    bool active = false;
    PlayerSelectReceiver selectedObject;
    GameObject player;
    VideoPlayer playerSelectScreen;
    Coroutine checkingCoroutine;
    Coroutine animationCoroutine;



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

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        player = GameManager.Instance.player;
        playerSelectScreen = GameManager.Instance.mainCamera.transform.Find("PlayerSelect").GetComponent<VideoPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (active) castRay();
	}
    #endregion



    protected override void OnLevelComplete() {
        activate();
    }

    protected override void OnNextLevel() {
        deactivate();
    }



    #region Public Functions
    /// <summary>
    /// Activates the PlayerSelect system
    /// </summary>
    public void activate() {
        active = true;
        if (playerSelectScreen != null) playerSelectScreen.Prepare();
    }

    /// <summary>
    /// Deactivates the PlayerSelect system
    /// </summary>
    public void deactivate() {
        active = false;
        deselectObject();
    }
    #endregion



    #region Private Functions
    /// <summary>
    /// Executed every update. This function triggers any further functionality of the PlayerSelect system
    /// </summary>
    void castRay() {
        RaycastHit hitInfo;
        // Raycast only hits "SelectableByView" Layer
        if (Physics.Raycast(player.transform.position, player.transform.forward, out hitInfo, 50f, 1 << 9)) {
            Debug.DrawLine(player.transform.position + player.transform.right * 0.03f, hitInfo.point, Color.cyan);
            selectObject(hitInfo.transform);
        }
        else {
            Debug.DrawRay(player.transform.position + player.transform.right * 0.03f, player.transform.forward * 50, Color.blue);
            deselectObject();
        }
    }

    void selectObject(Transform hit) {
        // Same object hit as before?
        if (hit.GetComponent<PlayerSelectReceiver>() == selectedObject) return;

        deselectObject();

        // Select the new object
        selectedObject = hit.GetComponent<PlayerSelectReceiver>();
        selectedObject.ActivateSelection();
        checkingCoroutine = StartCoroutine(checkSelection(selectedObject));

        // Start animation
        animationCoroutine = StartCoroutine(startAnimationCoroutine());
    }

    void deselectObject() {
        // Is there anything to deselect?
        if (selectedObject == null) return;

        // Deselect the current object
        selectedObject.DeactivateSelection();
        selectedObject = null;
        if (checkingCoroutine != null) StopCoroutine(checkingCoroutine);

        // Stop animation
        playerSelectScreen.GetComponent<MeshRenderer>().enabled = false;
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        playerSelectScreen.Stop();
    }
    #endregion



    #region Coroutines
    IEnumerator checkSelection(PlayerSelectReceiver checkedSelection) {
        for (float i = 0f; i < timeToSelect + 0.1f; i += Time.deltaTime) {            
            // If kept selected for defined time -> Do stuff
            if (i >= timeToSelect) {
                selectedObject.Execute();
            }
            yield return null;
        }
    }

    IEnumerator startAnimationCoroutine() {
        playerSelectScreen.Play();
        // Wait for video to start (needs a few frames for preparation of resources)
        for (int i = 0; i < 180; i++) {
            if (playerSelectScreen.isPrepared) break;
            yield return null;
            if (i == 179) {
                Debug.LogError("Couldn't load video for PlayerSelectScreen.");
                yield break;
            }
        }
        playerSelectScreen.GetComponent<MeshRenderer>().enabled = true;
    }
    #endregion
}
