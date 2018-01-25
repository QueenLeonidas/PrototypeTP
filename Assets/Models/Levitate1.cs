using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitate1 : MonoBehaviour {

    [Tooltip("Rotate speed in angle per second")]
    public float rotateSpeed = -12f;
    public float levitateSpeed = 5f;
    public float levitateHeight = 12f;

    // Use this for initialization
    void Start () {
        LeanTween.moveY(gameObject, transform.position.y + levitateHeight, levitateSpeed).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
	}
}
