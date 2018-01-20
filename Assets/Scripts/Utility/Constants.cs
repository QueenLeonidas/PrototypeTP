using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class containing all strings and constants used in the game
/// </summary>
public class Constants 
{
    #region Inputs
    public static readonly string INPUT_HORIZONTAL = "Horizontal";
    public static readonly string INPUT_VERTICAL = "Vertical";
    public static readonly string INPUT_JUMP = "Jump";
    public static readonly string INPUT_DEBUGMODE = "DebugMode";
    public static readonly string INPUT_INTERACT = "Interact";
    public static readonly string INPUT_TELEKINESIS = "Telekinesis";
    public static readonly string INPUT_TELEKINESISZ = "TelekinesisZ";
    public static readonly string INPUT_BUDDY_RESPAWN = "BuddyRespawn";
    public static readonly string INPUT_EXIT = "Exit";
    #endregion

    #region Tags and Layers
    // Tags
    public static readonly string TAG_BUDDY = "Buddy";
    public static readonly string TAG_PLAYER = "Player";
    public static readonly string TAG_ENERGY_ORB = "EnergyOrb";
    public static readonly string TAG_TELEKINETIC_OBJECT = "TelekineticObject";
    public static readonly string TAG_BUDDYSPAWN = "BuddySpawn";
    public static readonly string TAG_PLAYERTELEPORT = "PlayerTeleport";
    public static readonly string TAG_LEVEL = "Level";
    public static readonly string TAG_NEXT_LEVEL_PORTAL = "NextLevelPortal";
    #endregion

    #region Scenes
    public static readonly string SCENE_OPTIONS = "OptionsMenu";
    #endregion

    #region Sounds
    public static readonly string SOUND_BUDDY_SPAWN = "Buddy_Spawn";
    public static readonly string SOUND_BUDDY_DEATH = "Buddy_Death";
    public static readonly string SOUND_BUDDY_HAPPY = "Buddy_Happy";
    public static readonly string SOUND_BUDDY_CAMERA_ENTER = "Buddy_CameraEnter";
    public static readonly string SOUND_BUDDY_CAMERA_EXIT = "Buddy_CameraExit";
    public static readonly string SOUND_ORB_PICKUP = "Energy_Collect";
    public static readonly string SOUND_CUBE_PICKUP = "Cube_PickUp";
    public static readonly string SOUND_CUBE_FLOAT = "Cube_Floating_Loop";
    public static readonly string SOUND_CUBE_EMPTY = "Cube_Empty";
    public static readonly string SOUND_PORTAL_ACTIVE = "Portal_Activated";
    public static readonly string SOUND_TELEPORTATION = "Teleportation";
    public static readonly string SOUND_HOLO_ON = "Holo_On";
    public static readonly string SOUND_HOLO_OFF = "Holo_Off";
    public static readonly string SOUND_HOLO_LOOP = "Holo_Loop";
    #endregion

    #region Videos
    public static readonly string VIDEO_NONE = "None";
    public static readonly string VIDEO_BUDDY_MOVE = "buddy_move";
    public static readonly string VIDEO_BUDDY_JUMP = "buddy_jump";
    public static readonly string VIDEO_HOLO_BACKGROUND = "holo_screen";
    public static readonly string VIDEO_HOLO_CLOSE = "holo_screen_close";
    public static readonly string VIDEO_CUBE_HORIZONTAL = "horizontal_cube";
    public static readonly string VIDEO_CUBE_VERTICAL = "vertical_cube";
    public static readonly string VIDEO_CUBE_TIME = "time_cube";
    public static readonly string VIDEO_CUBE_DISTANCE = "space_cube";
    public static readonly string VIDEO_TELEKINESIS_Z = "perspective_cube_motion";
    #endregion

    #region Object References
    public static readonly string VRTK_PLAYER = "VRTK_SDK_Manager";
    #endregion
}