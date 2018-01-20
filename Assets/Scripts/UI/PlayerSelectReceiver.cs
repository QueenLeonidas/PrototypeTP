using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Offers functions that are triggered by the PlayerSelect system, if the player looks at the associated GameObject.
/// Every object that should be selectable by the PlayerSelect system needs a script derived from this.
/// </summary>
public abstract class PlayerSelectReceiver : SubscribedBehaviour {

    [SerializeField] Texture selectedStateTexture;
    Texture deselectedStateTexture;



    private void Start() {
         if (selectedStateTexture != null) deselectedStateTexture = GetComponent<MeshRenderer>().material.GetTexture("_MainTex");
    }



    public virtual void ActivateSelection() {
        if (selectedStateTexture != null) {
            GetComponent<Renderer>().material.SetTexture("_MainTex", selectedStateTexture);
        }
    }

    public virtual void DeactivateSelection() {
        if (selectedStateTexture != null) {
            GetComponent<Renderer>().material.SetTexture("_MainTex", deselectedStateTexture);
        }
    }

    public abstract void Execute();
}
