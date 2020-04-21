using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlashScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("P1_Fire_G")) {
			this.GetComponent<SpriteRenderer> ().enabled = false;
		}
	}
}
