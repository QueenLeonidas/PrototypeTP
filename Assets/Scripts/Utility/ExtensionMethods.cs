using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods {

    public static T findComponentInChildrenWithTag<T>(this Transform parent, string tag) where T : Component {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++) {
            if (children[i].tag == tag) {
                return children[i].GetComponent<T>();
            }
        }
        return null;
    }

    public static T[] findComponentsInChildrenWithTag<T>(this Transform parent, string tag) where T : Component {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        List<T> list = new List<T>();
        for (int i = 0; i < children.Length; i++) {
            if (children[i].tag.Contains(tag)) {
                list.Add(children[i].GetComponent<T>());
            }
        }
        T[] returnArray = new T[list.Count];
        list.CopyTo(returnArray);
        return returnArray;
    }

    public static bool inCentralView(this GameObject target) {
        if (Vector3.Angle(GameManager.Instance.player.transform.forward,
                          target.transform.position - GameManager.Instance.player.transform.position) <= 15
                          && (target.transform.position - GameManager.Instance.player.transform.position).magnitude <= 50) {
            return true;
        }
        else {
            return false;
        }
    }

    public static bool inView(this GameObject target) {
        if (Vector3.Angle(GameManager.Instance.player.transform.forward,
                          target.transform.position - GameManager.Instance.player.transform.position) <= 55
                          && (target.transform.position - GameManager.Instance.player.transform.position).magnitude <= 50) {
            return true;
        }
        else {
            return false;
        }
    }
}
