using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// 
/// </summary>
public class VideoLibrary : SubscribedBehaviour {

    #region Variable Declarations
    [SerializeField] VideoClip[] videoClips;

    public static VideoLibrary Instance;
    #endregion



    #region Unity Event Functions
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

    private void Start () {
        
	}
	
	private void Update () {
		
	}
    #endregion



    #region Public Functions
    public VideoClip getVideoClip(string name) {
        foreach (VideoClip clip in videoClips) {
            if (clip.name == name) return clip;
        }
        Debug.LogError("VideoClip \"" + name + "\" not found.");
        return null;
    }
    #endregion



    #region Private Functions

    #endregion
}
