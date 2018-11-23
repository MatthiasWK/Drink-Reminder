﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrinkMeter : MonoBehaviour {
    GameObject Timer;
	// Use this for initialization
	void Start () {
        Timer = GameObject.Find("Timer and Bluetooth");
        GetComponent<Slider>().maxValue = Timer.GetComponent<timer_controller>().baseTime;
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<Slider>().value = Timer.GetComponent<timer_controller>().remainder;
	}
}