﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitate : MonoBehaviour {

    [Tooltip("Rotate speed in angle per second")]
    public float rotateSpeed = 6f;
    public float levitateSpeed = 8f;
    public float levitateHeight = 16f;

    // Use this for initialization
    void Start () {
        LeanTween.moveY(gameObject, transform.position.y + levitateHeight, levitateSpeed).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
	}
}
