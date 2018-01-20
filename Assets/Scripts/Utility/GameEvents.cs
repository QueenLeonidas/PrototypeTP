using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Provides all Custom Events we are using
/// </summary>
public class GameEvents {

    // Initializing the Delegates for the game events
    public delegate void GameEvent();

    // Create references for our delegates
    // This event is triggered when the player is free to move around
    public static event GameEvent NextLevel;
    public static event GameEvent LevelComplete;
    public static event GameEvent OrbsCollected;



    /// <summary>
    /// Helper Function to start the exploration mode from within other classes
    /// </summary>
    public static void StartNextLevel() {
        NextLevel();
    }
    public static void StartLevelComplete() {
        LevelComplete();
    }
    public static void StartOrbsCollected() {
        OrbsCollected();
    }
}
