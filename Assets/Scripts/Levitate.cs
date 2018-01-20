using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitate : MonoBehaviour {

    [Tooltip("Rotate speed in angle per second")]
    [SerializeField] float rotateSpeed = 45f;

	// Use this for initialization
	void Start () {
        LeanTween.moveY(gameObject, transform.position.y + 0.5f, 1f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
	}
}
