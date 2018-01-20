using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : SubscribedBehaviour {

    #region Variable Declarations
    public static TutorialManager Instance;
    HoloScreen explanationHoloScreen;
    bool buddyMoveExplained = false;
    bool buddyJumpExplained = false;
    bool horizontalCubeExplained = false;
    bool verticalCubeExplained = false;
    //bool timeCubeExplained = false;
    bool distanceCubeExplained = false;
    bool telekinesisZExplained = false;
    [SerializeField] Transform firstObstacle;
    [SerializeField] Transform firstHorizontalCube;
    [SerializeField] Transform firstVerticalCube;
    //[SerializeField] Transform firstTimeCube;
    [SerializeField] Transform firstDistanceCube;
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

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        explanationHoloScreen = GameManager.Instance.explanationHoloScreen;
	}
	
	// Update is called once per frame
	void Update () {
        if (!buddyMoveExplained && GameManager.Instance.getCurrentLevel() == 1) StartCoroutine(checkBuddyMove());
        if (!buddyJumpExplained && GameManager.Instance.getCurrentLevel() == 2) StartCoroutine(checkBuddyJump());
        if (!horizontalCubeExplained && GameManager.Instance.getCurrentLevel() == 3) StartCoroutine(checkHorizontalCube());
        if (!verticalCubeExplained && horizontalCubeExplained && GameManager.Instance.getCurrentLevel() == 3) StartCoroutine(checkVerticalCube());
        //if (!timeCubeExplained && GameManager.Instance.getCurrentLevel() == 3) StartCoroutine(checkTimeCube());
        if (!distanceCubeExplained && GameManager.Instance.getCurrentLevel() == 5) StartCoroutine(checkDistanceCube());
        if (!telekinesisZExplained && distanceCubeExplained && GameManager.Instance.getCurrentLevel() == 5) StartCoroutine(checkTelekinesisZ());
    }
    #endregion



    #region Public Functions

    #endregion



    #region Private Functions
    private bool buddyCloseTo(Transform target, float distance) {
        if (Vector3.Distance(GameManager.Instance.buddy.transform.position, target.position) < distance) {
            return true;
        }
        return false;
    }
    #endregion



    #region Coroutines
    IEnumerator checkBuddyMove() {
        if (!buddyMoveExplained && GameManager.Instance.buddy.inCentralView()) {
            buddyMoveExplained = true;
            explanationHoloScreen.playVideo(Constants.VIDEO_BUDDY_MOVE);
            int i = 0;
            while (Input.GetAxis(Constants.INPUT_HORIZONTAL) == 0 && Input.GetAxis(Constants.INPUT_VERTICAL) == 0) {
                yield return null;
                i++;
                if (i > 18000000) {
                    Debug.LogError("TutorialManager waiting for stop condition for video for too long. Breaking loop.");
                    break;
                }
            }
            explanationHoloScreen.stopCurrentVideo();
        }
    }

    IEnumerator checkBuddyJump() {
        if (!buddyJumpExplained && buddyCloseTo(firstObstacle, 4f)) {
            buddyJumpExplained = true;
            explanationHoloScreen.playVideo(Constants.VIDEO_BUDDY_JUMP);
            int i = 0;
            while (!Input.GetButtonDown(Constants.INPUT_JUMP)) {
                yield return null;
                i++;
                if (i > 18000000) {
                    Debug.LogError("TutorialManager waiting for stop condition for video for too long. Breaking loop.");
                    break;
                }
            }
            explanationHoloScreen.stopCurrentVideo();
        }
    }

    IEnumerator checkHorizontalCube() {
        if (!horizontalCubeExplained && firstHorizontalCube.GetComponent<TelekineticObject>() == TelekinesisController.Instance.getSelectedObject()) {
            horizontalCubeExplained = true;
            explanationHoloScreen.playVideo(Constants.VIDEO_CUBE_HORIZONTAL);
            int i = 0;
            while (!(TelekinesisController.Instance.getSelectedObject() == firstHorizontalCube.GetComponent<TelekineticObject>()
                   && TelekinesisController.Instance.telekinesisInProgress)) {
                yield return null;
                i++;
                if (i > 18000000) {
                    Debug.LogError("TutorialManager waiting for stop condition for video for too long. Breaking loop.");
                    break;
                }
            }
            explanationHoloScreen.stopCurrentVideo();
        }
    }

    IEnumerator checkVerticalCube() {
        if (!verticalCubeExplained && firstVerticalCube.GetComponent<TelekineticObject>() == TelekinesisController.Instance.getSelectedObject()) {
            verticalCubeExplained = true;
            explanationHoloScreen.playVideo(Constants.VIDEO_CUBE_VERTICAL);
            int i = 0;
            while (!(TelekinesisController.Instance.getSelectedObject() == firstVerticalCube.GetComponent<TelekineticObject>()
                   && TelekinesisController.Instance.telekinesisInProgress)) {
                yield return null;
                i++;
                if (i > 18000000) {
                    Debug.LogError("TutorialManager waiting for stop condition for video for too long. Breaking loop.");
                    break;
                }
            }
            explanationHoloScreen.stopCurrentVideo();
        }
    }

    //IEnumerator checkTimeCube() {
    //    if (!timeCubeExplained && firstTimeCube.GetComponent<TelekineticObject>() == TelekinesisController.Instance.getSelectedObject()) {
    //        timeCubeExplained = true;
    //        explanationHoloScreen.playVideo(Constants.VIDEO_CUBE_TIME);
    //        int i = 0;
    //        while (!(TelekinesisController.Instance.getSelectedObject() == firstTimeCube.GetComponent<TelekineticObject>()
    //               && TelekinesisController.Instance.telekinesisInProgress)) {
    //            yield return null;
    //            i++;
    //            if (i > 18000000) {
    //                Debug.LogError("TutorialManager waiting for stop condition for video for too long. Breaking loop.");
    //                break;
    //            }
    //        }
    //        explanationHoloScreen.stopCurrentVideo();
    //    }
    //}

    IEnumerator checkDistanceCube() {
        if (!distanceCubeExplained && firstDistanceCube.GetComponent<TelekineticObject>() == TelekinesisController.Instance.getSelectedObject()) {
            distanceCubeExplained = true;
            explanationHoloScreen.playVideo(Constants.VIDEO_CUBE_DISTANCE);
            int i = 0;
            while (!(TelekinesisController.Instance.getSelectedObject() == firstDistanceCube.GetComponent<TelekineticObject>()
                   && TelekinesisController.Instance.telekinesisInProgress)) {
                yield return null;
                i++;
                if (i > 18000000) {
                    Debug.LogError("TutorialManager waiting for stop condition for video for too long. Breaking loop.");
                    break;
                }
            }
            explanationHoloScreen.stopCurrentVideo();
        }
    }

    IEnumerator checkTelekinesisZ() {
        if (!telekinesisZExplained && firstDistanceCube.GetComponent<TelekineticObject>() == TelekinesisController.Instance.getSelectedObject()) {
            telekinesisZExplained = true;
            explanationHoloScreen.playVideo(Constants.VIDEO_TELEKINESIS_Z);
            int i = 0;
            while (!(TelekinesisController.Instance.telekinesisInProgress
                     && Input.GetAxis(Constants.INPUT_TELEKINESISZ) != 0)) {
                yield return null;
                i++;
                if (i > 18000000) {
                    Debug.LogError("TutorialManager waiting for stop condition for video for too long. Breaking loop.");
                    break;
                }
            }
            explanationHoloScreen.stopCurrentVideo();
        }
    }
    #endregion
}
