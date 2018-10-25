using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Profile : MonoBehaviour {

	public static string currentUser;
	char[] test;
	//char[] tmp;

	void Start(){
        currentUser = GetComponentInChildren<Text>().text;

		Debug.Log("names: "+ currentUser);
		gameObject.name = currentUser;

	}

	void OnMouseDown(){
		//Debug.Log("ich bin babababa: " + this.gameObject.name);

		GameController gc = new GameController();
		gc.loadProfil(this.gameObject.name);
		GameController.tmp_Name = this.gameObject.name;
	}
		
}
