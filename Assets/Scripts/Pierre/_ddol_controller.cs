using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _ddol_controller : MonoBehaviour {

	public bool instantly;

	// Use this for initialization
	void Start () {
		if (instantly)
			SceneManager.LoadScene ("Menue");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
