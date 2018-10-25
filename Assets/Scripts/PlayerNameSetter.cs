using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameSetter : MonoBehaviour {

    private void OnEnable()
    {
        gameObject.GetComponent<Text>().text = "Logged in as: " + GameController.tmp_Name;
    }
}
