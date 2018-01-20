using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Manages all background audio playing in the game. This class is a singleton and won't be destroyed when loading a new scene.
/// </summary>
public class AudioManager : SubscribedBehaviour {

    #region Variable Declarations
    public static AudioManager Instance;

    [Header("Sound Volumes")]
    [Range(0, 1)]
    public float portalActiveVolume = 1;
    [Range(0, 1)]
    public float teleportVolume = 1;
    [Range(0, 1)]
    public float holoOnVolume = 1;
    [Range(0, 1)]
    public float holoOffVolume = 1;
    [Range(0, 1)]
    public float holoLoopVolume = 1;

    [Header("References")]
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] AudioClip[] audioClips;
    grumbleAMP grumbleAMP;

    // Private Variables
    AudioSource[] audioSources;
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

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of an AudioManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        grumbleAMP = GameObject.FindObjectOfType<grumbleAMP>();
        CreateAudioSources();
        GetAudioSource(Constants.SOUND_CUBE_FLOAT).loop = true;
        GetAudioSource(Constants.SOUND_HOLO_LOOP).loop = true;

        grumbleAMP.PlaySong(0, GameManager.Instance.getCurrentLevel(), 3f);
    }
	
	private void Update() {
		
	}
    #endregion



    #region Custom Event Functions
    // Every child of SubscribedBehaviour can implement these
    protected override void OnLevelComplete() {
        PlayAudio(Constants.SOUND_PORTAL_ACTIVE, portalActiveVolume);
    }

    protected override void OnNextLevel() {
        PlayAudio(Constants.SOUND_TELEPORTATION, teleportVolume);
        grumbleAMP.CrossFadeToNewLayer(GameManager.Instance.getCurrentLevel());
    }
    #endregion



    #region Public Functions
    // Doesn't seem to work with iTween. Maybe try a different library
    //public void FadeOutAndStop(string sound, float fadeOutTime) {
    //    iTween.AudioTo(GetAudioSource(sound), iTween.Hash("volume", 0f, "time", fadeOutTime, "easetype", iTween.EaseType.easeInOutQuad));
    //}

    public void PlayAudio(string sound, float volume) {
        AudioSource src = GetAudioSource(sound);
        src.volume = volume;
        src.Play();
    }

    public void StopAudio(string sound) {
        GetAudioSource(sound).Stop();
    }

    public AudioClip GetAudioClip(string sound) {
        foreach (AudioClip clip in audioClips) {
            if (clip.name == sound) {
                return clip;
            }
        }
        Debug.LogError("AudioClip \"" + sound + "\" couldn't be found.");
        return null;
    }

    public AudioSource GetAudioSource(string sound) {
        foreach (AudioSource src in audioSources) {
            if (src.clip.name == sound) {
                return src;
            }
        }
        Debug.LogError("AudioSource for \"" + sound + "\" couldn't be found.");
        return null;
    }
    #endregion



    #region Private Functions
    private void CreateAudioSources() {
        for (int i = 0; i < audioClips.Length; i++) {
            gameObject.AddComponent<AudioSource>();
        }
        audioSources = GetComponents<AudioSource>();
        for (int i = 0; i < audioClips.Length; i++) {
            SetupAudioSource(audioSources[i]);
            audioSources[i].clip = audioClips[i];
        }
    }

    private void SetupAudioSource(AudioSource audioSrc) {
        audioSrc.outputAudioMixerGroup = masterMixer.FindMatchingGroups("2D Sounds")[0];
        audioSrc.volume = 1f;
        audioSrc.loop = false;
        audioSrc.playOnAwake = false;
    }
    #endregion
}
