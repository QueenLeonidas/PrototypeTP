using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Base class for all HoloScreens in game.
/// Important note: The Coroutines to start the screens throw NullReferenceExceptions when being invoked out of start!
/// </summary>
[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(AudioSource))]
public class HoloScreen : SubscribedBehaviour {

    #region Variable Declarations
    [Header("Sound Volumes")]
    [Range(0, 1)]
    [SerializeField]
    float holoOnVolume = 1;
    [Range(0, 1)]
    [SerializeField]
    float holoLoopVolume = 1;
    [Range(0, 1)]
    [SerializeField]
    float holoOffVolume = 1;

    VideoPlayer videoPlayer;
    VideoPlayer background;
    VideoPlayer closing;
    AudioSource audioSource;
    PlayerSelectReceiver executeOnVideoFinish;
    bool videoPlaying = false;
    bool videoStarting = false;
    bool executeOnVideoFinishExecuted = false;
    #endregion



    #region Unity Event Functions
    private void Start() {
        videoPlayer = GetComponent<VideoPlayer>();
        background = transform.Find("Background").GetComponent<VideoPlayer>();
        closing = transform.Find("Close").GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();
        executeOnVideoFinish = GetComponent<PlayerSelectReceiver>();
    }

    private void Update() {
        if (executeOnVideoFinish != null && !executeOnVideoFinishExecuted && videoPlaying && !videoStarting && !videoPlayer.isPlaying) {
            executeOnVideoFinish.Execute();
            executeOnVideoFinishExecuted = true;
        }
    }
    #endregion



    #region Public Functions
    /// <summary>
    /// Plays the currently selected VideoClip
    /// </summary>
    public void playVideo() {
        StartCoroutine(playVideoCoroutine("None"));
    }

    /// <summary>
    /// Sets the defined video as VideoClip and plays it
    /// Note: Throws NullReferenceException when being called from Start()
    /// </summary>
    public void playVideo(string name) {
        StartCoroutine(playVideoCoroutine(name));
    }

    /// <summary>
    /// Stops the current video if any should be playing right now
    /// </summary>
    public void stopCurrentVideo() {
        StartCoroutine(stopCurrentVideoClipCoroutine());
    }
    #endregion



    #region Coroutines
    /// <summary>
    /// Plays background video, activates MeshRenderer and waits for background video to finish
    /// </summary>
    IEnumerator playVideoCoroutine(string name) {
        // Wait if a video is already playing (kind of queue)
        bool abort = false;
        if (videoPlaying) {
            for (int i = 0; i < 900; i++) {
                yield return null;
                if (!videoPlaying) break;
                if (i == 899) {
                    Debug.LogError("Couldn't play video \"" + name + "\". Aborting...");
                    abort = true;
                }
            }
        }
        // Abort if waited too long
        if (abort) yield break;

        videoPlaying = true;
        videoStarting = true;
        // Play background video and activate the meshRenderer when video starts
        background.Play();
        // Wait for video to start (needs a few frames for preparation of resources)
        for (int i = 0; i < 180; i++) {
            if (background.isPrepared) break;
            yield return null;
            if (i == 179) Debug.LogError("Couldn't load video for backgroundPlayer.");
        }
        AudioManager.Instance.PlayAudio(Constants.SOUND_HOLO_ON, holoOnVolume);
        audioSource.Play();
        background.GetComponent<MeshRenderer>().enabled = true;
        // Wait for video to finish
        for (int i = 0; i < 1800; i++) {
            yield return null;
            if (!background.isPlaying) break;
        }

        // Play foreground video and activate the meshRenderer when video starts
        if (name != "None") {
            videoPlayer.clip = VideoLibrary.Instance.getVideoClip(name);
        }
        videoPlayer.Play();
        // Wait for video to start (needs a few frames for preparation of resources)
        for (int i = 0; i < 180; i++) {
            if (videoPlayer.isPrepared) break;
            yield return null;
        }
        videoPlayer.GetComponent<MeshRenderer>().enabled = true;
        videoStarting = false;
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator stopCurrentVideoClipCoroutine() {
        // Check if there's a video to close
        if (!videoPlaying) yield break;

        // Wait if a video is currently starting
        bool abort = false;
        if (videoStarting) {
            for (int i = 0; i < 900; i++) {
                yield return null;
                if (!videoStarting) break;
                if (i == 899) {
                    Debug.LogError("Couldn't stop video \"" + videoPlayer.clip.name + "\". Aborting...");
                    abort = true;
                }
            }
        }
        // Abort if waited too long
        if (abort) yield break;

        // Stop and deactivate foreground video
        videoPlayer.GetComponent<MeshRenderer>().enabled = false;
        videoPlayer.Stop();

        // Start video clip
        closing.Play();
        // Wait for video to start (needs a few frames for preparation of resources)
        for (int i = 0; i < 180; i++) {
            if (closing.isPrepared) break;
            yield return null;
            if (i == 179) Debug.LogError("Couldn't load video \"" + name + "\".");
        }
        AudioManager.Instance.PlayAudio(Constants.SOUND_HOLO_OFF, holoOffVolume);
        closing.GetComponent<MeshRenderer>().enabled = true;
        background.GetComponent<MeshRenderer>().enabled = false;
        background.Stop();
        // Wait for video to finish
        for (int i = 0; i < 1800; i++) {
            yield return null;
            if (!closing.isPlaying) break;
        }

        audioSource.Stop();
        closing.GetComponent<MeshRenderer>().enabled = false;
        closing.Stop();
        videoPlaying = false;
    }
    #endregion
}
