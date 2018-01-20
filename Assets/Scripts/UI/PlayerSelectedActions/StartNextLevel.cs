using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNextLevel : PlayerSelectReceiver {

    public override void Execute() {
        GameEvents.StartNextLevel();
    }
}
