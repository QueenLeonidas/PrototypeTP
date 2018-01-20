using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ShowStory : PlayerSelectReceiver {

    [SerializeField] MainMenu mainMenuScript;

    public override void Execute() {
        PlayerSelect.Instance.deactivate();
        mainMenuScript.turnOffButtons();
        mainMenuScript.showStory();
    }
}