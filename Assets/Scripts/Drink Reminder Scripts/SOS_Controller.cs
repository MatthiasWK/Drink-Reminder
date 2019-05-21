using UnityEngine;
using System.Collections;

public class SOS_Controller : MonoBehaviour {

    public static int current_page;

    public void SOSScreen()
    {
        current_page = Application.loadedLevel;
        Application.LoadLevel("SOS");
    }

}
