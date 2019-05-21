using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowAnimator : MonoBehaviour {
    public float speed;
	// Rotate the star designating bonus objects
	void Update () {
        transform.Rotate(0, 0, speed *Time.deltaTime);

	}
}
