using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TelekinesisController : SubscribedBehaviour {

    #region Variable Declarations
    public static TelekinesisController Instance;

    public bool telekinesisInProgress = false;
    [SerializeField]
    float zAxisMultiplier = 0.1f;

    TelekineticObject selectedObject;
    float distanceToSelectedObject;
    GameObject player;
    float zAxisInput;
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



    public TelekineticObject getSelectedObject() {
        return selectedObject;
    }



    private void Start() {
        player = GameManager.Instance.player;

    }

    private void FixedUpdate() {
        if (telekinesisInProgress) telekineticAction();
    }

    private void Update() {
        checkFocus();

        checkInput();
    }

    private void OnDrawGizmos() {
        if (selectedObject != null) {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(selectedObject.transform.position, 1f);
        }
    }
    #endregion



    #region Public Functions
    
    #endregion



    #region Private Functions
    private void checkInput() {
        if (Input.GetButtonDown(Constants.INPUT_TELEKINESIS) && selectedObject != null && selectedObject.Telekinizable) {
            telekinesisInProgress = true;
            selectedObject.startTelekineticMovement();
        }

        if (Input.GetButtonUp(Constants.INPUT_TELEKINESIS) && selectedObject) {
            telekinesisInProgress = false;
            selectedObject.stopTelekineticMovement();
        }

        zAxisInput = Input.GetAxis(Constants.INPUT_TELEKINESISZ);
    }

    private void checkFocus() {
        // Don't check for new objects when we are already telekinesing one
        if (telekinesisInProgress) return;

        RaycastHit hitInfo;
        if (Physics.Raycast(player.transform.position, player.transform.forward, out hitInfo, 50f, 1 << 8) && hitInfo.transform.GetComponent<TelekineticObject>().Telekinizable) {
            Debug.DrawLine(player.transform.position, hitInfo.point, Color.green);
            selectObject(hitInfo.transform.GetComponent<TelekineticObject>());
        }
        else {
            Debug.DrawRay(player.transform.position, player.transform.forward * 50, Color.red);
            deselectObject();
        }
    }

    private void telekineticAction() {
        // Calculate position depending on point of view of the player and add zAxis Input from the controller
        distanceToSelectedObject -= zAxisInput * zAxisMultiplier;
        Vector3 targetPosition = player.transform.position + player.transform.forward * distanceToSelectedObject;

        selectedObject.moveTelekineticObject(targetPosition);
    }

    private bool selectObject(TelekineticObject selectedObject) {
        // Error handling. We should actually only hit TelekineticObjects
        if (selectedObject == null) {
            deselectObject();
            return false;
        }

        // New TelekineticObject detected
        if (selectedObject != this.selectedObject) {
            deselectObject();
            this.selectedObject = selectedObject;
            distanceToSelectedObject = (selectedObject.transform.position - player.transform.position).magnitude;
            selectedObject.transform.GetChild(0).GetComponent<cakeslice.Outline>().enabled = true;
        }
        return true;
    }

    private void deselectObject() {
        if (selectedObject != null) {
            selectedObject.transform.GetChild(0).GetComponent<cakeslice.Outline>().enabled = false;
            selectedObject = null;
            distanceToSelectedObject = 0;
        }
    }
    #endregion
}
