using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : PlayerSelectReceiver {

    override public void Execute() {
        GameManager.Instance.ExitGame();
    }
}
