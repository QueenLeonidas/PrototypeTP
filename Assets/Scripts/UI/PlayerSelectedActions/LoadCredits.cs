using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCredits : PlayerSelectReceiver {

    public override void Execute() {
        GameManager.Instance.setCurrentLevel(GameManager.Instance.getLevelCount() - 2);
        GameEvents.StartNextLevel();
    }
}
