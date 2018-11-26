using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompanionController : MonoBehaviour {
    GameObject Timer;
    public Text CompanionText;
    public Image CompanionBubble;

	// Use this for initialization
	void Start () {
        Timer = GameObject.Find("Timer and Bluetooth");
        GetComponent<Slider>().maxValue = Timer.GetComponent<timer_controller>().baseTime;
    }
	
	// Update Slider value once per frame
	void Update () {
        GetComponent<Slider>().value = Timer.GetComponent<timer_controller>().remainder;
	}

    public void SayBomb()
    {
        CompanionText.text = "Wirf die Wasserbombe!";
    }

    public void ToggleSpeech(bool s)
    {
        CompanionBubble.gameObject.SetActive(s);
        CompanionText.gameObject.SetActive(s);
    }
}
