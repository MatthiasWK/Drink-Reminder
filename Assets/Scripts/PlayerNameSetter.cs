using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameSetter : MonoBehaviour {

    /// <summary>
    /// Show the logged in player's name
    /// </summary>
    private void OnEnable()
    {
        gameObject.GetComponent<Text>().text = "Angemeldet als: " + GameController.tmp_Name;
    }
}
