using UnityEngine;
using System.Collections;

public class changeText : MonoBehaviour {

    GameInformation test = new GameInformation();

    // Use this for initialization
    void Start () {
        
		if(name == "Score")
			GetComponent<TextMesh>().text = test.getScore().ToString();
		else
			GetComponent<TextMesh>().text = GameController.tmp_Name;

    }
	
	// Update is called once per frame
	void Update () {
        if (name == "Score")
            GetComponent<TextMesh>().text = test.getScore().ToString();

    }
}
