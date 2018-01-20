using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Manages the overall flow of the game and scene loading. This class is a singleton and won't be destroyed when loading a new scene.
/// </summary>
public class GameManager : SubscribedBehaviour {

    #region Variable Declarations
    [Header("VR Setup")]
    public VRSetups vRSetup;

    [Header("Level Parameters")]
    [SerializeField] private int currentLevel = 0;
    private Level[] levelList;

    [Header("Can't touch this")]
    public GameObject player;
    public GameObject buddy;
    public Camera mainCamera;
    public HoloScreen explanationHoloScreen;
    

    private int buddyEnergyLevel;
    private Vector3 lastEnergyOrbPosition;
    PlayerSelect playerSelect;

    public static GameManager Instance;
    #endregion



    #region Unity Event Functions
    //Awake is always called before any Start functions
    void Awake() {
        //Check if instance already exists
        if(Instance == null)

            //if not, set instance to this
            Instance = this;

        //If instance already exists and it's not this:
        else if(Instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        findPlayer();
    }

    private void Start() {
        initializeScene();

        if (currentLevel == 0) {
            playerSelect.activate();
        }
    }

    private void Update() {
        if (Input.GetButtonDown(Constants.INPUT_EXIT)) {
            ExitGame();
        }
    }
    #endregion



    #region Custom Event Functions
    // Every child of SubscribedBehaviour can implement these
    protected override void OnNextLevel() {
        currentLevel++;
        buddyEnergyLevel = 0;
        stopBuddyActionAtLevelEnd();
        respawnPlayer();
        checkFoLastLevelreached();
    }

    protected override void OnLevelComplete() {
        
    }

    protected override void OnOrbsCollected() {
        activateNextLevelPortal();
    }
    #endregion



    #region Public Functions
    #region Scene Management
    /// <summary>
    /// Loads a scene by name
    /// </summary>
    public void LoadSceneByName(string name) {
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// Loads the next scene in the build index
    /// </summary>
    public void LoadNextScene() {
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        if(activeScene + 1 < SceneManager.sceneCountInBuildSettings) {
            SceneManager.LoadScene(activeScene + 1);
        } else {
            Debug.LogError("No more levels in build index to be loaded.");
        }
    }

    /// <summary>
    /// Quits the application or exits play mode when in editor
    /// </summary>
    public void ExitGame() {
        Debug.Log("Exiting the game.");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    #endregion

    public void increaseBuddyEnergyLevel(int amount) {
        buddyEnergyLevel += amount;

        try {
            if (buddyEnergyLevel >= getCurrentOrbsInLevel()) {
                GameEvents.StartOrbsCollected();
            }
        }
        catch (System.IndexOutOfRangeException e) {
            Debug.LogError("Number of orbs for level " + currentLevel + " not defined" + " ." + e);
        }
    }

    public void setLastEnergyOrbPosition(Vector3 collectedOrbPosition) {
        lastEnergyOrbPosition = collectedOrbPosition;
    }

    public Transform getCurrentBuddySpawn() {
        try {
            if (levelList[currentLevel].buddySpawnPoint == null) Debug.LogError("buddySpawnPoint == null");
            return levelList[currentLevel].buddySpawnPoint;
        }
        catch (System.IndexOutOfRangeException e) {
            Debug.LogError("levelList doesn't contain currentLevel: " + currentLevel + " ." + e);
        }
        return null;
    }

    public Transform getCurrentPlayerTeleportTransform() {
        try {
            if (levelList[currentLevel].playerTeleportPosition == null) Debug.LogError("playerTeleportPosition == null");
            return levelList[currentLevel].playerTeleportPosition;
        }
        catch (System.IndexOutOfRangeException e) {
            Debug.LogError("levelList doesn't contain currentLevel: " + currentLevel + " ." + e);
        }
        return null;
    }

    public GameObject getNextLevelPortal() {
        return levelList[currentLevel].transform.findComponentInChildrenWithTag<Transform>(Constants.TAG_NEXT_LEVEL_PORTAL).gameObject;
    }

    public int getCurrentOrbsInLevel() {
        try {
            if (levelList[currentLevel].orbsInLevel == 0) Debug.LogError("orbsInLevel == 0");
            return levelList[currentLevel].orbsInLevel;
        }
        catch (System.IndexOutOfRangeException e) {
            Debug.LogError("levelList doesn't contain currentLevel: " + currentLevel + " ." + e);
        }
        return -1;
    }

    public int getCurrentLevel() {
        return currentLevel;
    }

    public void setCurrentLevel(int level) {
        if (level >= 0 && level < levelList.Length) {
            currentLevel = level;
        }
        else {
            Debug.LogError("Trying to set a level that doesn't exist.");
        }
    }

    public void respawnPlayer() {
        FadePanel.Instance.fadeOut(() => {
            GameObject.Find(Constants.VRTK_PLAYER).transform.position = getCurrentPlayerTeleportTransform().position;
            FadePanel.Instance.fadeIn();
        });
    }

    public int getLevelCount() {
        return levelList.Length;
    }
    #endregion



    #region Private Functions
    private void initializeScene() {
        mainCamera = player.GetComponent<Camera>();
        playerSelect = GetComponent<PlayerSelect>();
        setupLevelList();
        findBuddy();
        findExplanationHoloScreen();
        setupAllCanvas();
    }

    private void findPlayer() {
        if (vRSetup == VRSetups.Oculus) {
            player = GameObject.Find(Constants.VRTK_PLAYER).transform.Find("Oculus").GetChild(0).GetChild(0).Find("CenterEyeAnchor").gameObject;
        }
        else if (vRSetup == VRSetups.Simulator) {
            player = GameObject.Find(Constants.VRTK_PLAYER).transform.Find("Simulator").GetChild(0).Find("Neck").GetChild(0).gameObject;
        }
        else {
            Debug.LogError("Player not found!");
        }
    }

    private void findBuddy() {
        buddy = GameObject.FindGameObjectWithTag(Constants.TAG_BUDDY).gameObject;
    }
    private void activateNextLevelPortal() {
        GameObject nextLevelPortal = getNextLevelPortal();
        nextLevelPortal.transform.Find("NextLevelParticle").gameObject.SetActive(true);
        nextLevelPortal.transform.Find("TriggerForPlayer").gameObject.SetActive(true);
    }
    

    private void stopBuddyActionAtLevelEnd() {
        buddy.GetComponent<BuddyController>().stopLevelCompleteAnimation();
    }
    private void deactivateNextLevelPortal() {
        GameObject nextLevelPortal = getNextLevelPortal();
        nextLevelPortal.transform.Find("NextLevelPortal/NextLevelParticle").gameObject.SetActive(false);
        nextLevelPortal.transform.Find("TriggerForPlayer").gameObject.SetActive(false);
    }

    private void checkFoLastLevelreached() {
        int lastLevelNumber;
        Level activeLevel;

        lastLevelNumber = levelList.Length-1;
        activeLevel = levelList[currentLevel];
        
        if(activeLevel.levelNumber == lastLevelNumber) {
            buddy.transform.LookAt(player.transform.position, Vector3.up);

            buddy.GetComponent<BuddyController>().setGameEndAnimation();
            deactivatePlayerControls();
        }
    }

    private void deactivatePlayerControls() {
        buddy.GetComponent<ThirdPersonCharacter>().enabled = false;
        buddy.GetComponent<ThirdPersonUserControl>().enabled = false;
    }

    private void setupAllCanvas() {
        Canvas[] canvasInScene = GameObject.FindObjectsOfType<Canvas>();
        for (int i = 0; i < canvasInScene.Length; i++) {
            if (canvasInScene[i].renderMode == RenderMode.ScreenSpaceOverlay) {
                canvasInScene[i].renderMode = RenderMode.ScreenSpaceCamera;
                canvasInScene[i].worldCamera = mainCamera;
            }
            if(canvasInScene[i].name == "VideoCanvas") {
                canvasInScene[i].worldCamera = mainCamera;

            }
        }
    }

    private void setupLevelList() {
        Level[] helperArray = GameObject.FindObjectsOfType<Level>();
        levelList = new Level[helperArray.Length];
        for (int i = 0; i < helperArray.Length; i++) {
            for (int j = 0; j < helperArray.Length; j++) {
                if (helperArray[j].levelNumber == i) {
                    levelList[i] = helperArray[j];
                }
            }
        }
    }

    private void findExplanationHoloScreen() {
        if (vRSetup == VRSetups.Oculus) {
            explanationHoloScreen = player.transform.Find("ExplanationHoloScreen").GetComponent<HoloScreen>();
        }
        else if (vRSetup == VRSetups.Simulator) {
            explanationHoloScreen = player.transform.Find("ExplanationHoloScreen").GetComponent<HoloScreen>();
        }
        else {
            Debug.LogError("VRSetup undefined");
        }

        // Error handling
        if (explanationHoloScreen == null) Debug.LogError("HoloScreen not found!");
    }
    #endregion



    #region Coroutines
    #endregion
}