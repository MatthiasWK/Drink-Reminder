﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class timer_controller : MonoBehaviour
{

    public GameObject notification, canvas;
    public GameObject textMessage;
    //private string tmpMessage;
    public int baseTime, remainder;
    public bool running, cancelQueue, blocked;
    public static bool pause = false;

    AndroidJavaClass jc;
    string javaMessage = "";

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(canvas);
        running = cancelQueue = blocked = false;
        Start(baseTime);
    }

    void Update()
    {
        if (remainder > 0)
            remainder -= (int) Time.deltaTime;

        //TODO Block Game
        if (BroadcastReceiver.hasDrunk && pause)
        {
            BroadcastReceiver.hasDrunk = false;
            StartCoroutine(changeMessage());
        }
    }

    public void Start(int time)
    {
        if (!blocked)
        {
            if (!running)
                running = true;

            remainder = time;
            Invoke("timeStep", 1f);
        }
    }

    public void cancel()
    {
        cancelQueue = true;
    }

    public void restart(int time)
    {
        if (!blocked && running)
        {
            remainder = time;
        }
        else if (!blocked)
        {
            Start(time);
        }
    }

    public void unblock()
    {
        blocked = false;
    }

    public void trigger()
    {
        blocked = false;
        pause = true;
        canvas.SetActive(true);

        if (SceneManager.GetActiveScene().name == "mainGame")
            Constants.TriggerStop = true;
    }

    public void unTrigger()
    {
        pause = false;
        canvas.SetActive(false);
        restart(baseTime);
        //if (Application.loadedLevel.Equals("Balloon"))
        //    GameConfig_Slice.cupStatus = true;

        if (SceneManager.GetActiveScene().name == "mainGame")
            Constants.TriggerGo = true;

    }

    /// <summary>
    /// Called when using manual Input to continue the game instead of waiting for gadget input
    /// </summary>
    public void Continue()
    {
        StartCoroutine(changeMessage());
    }

    private void timeStep()
    {
        if (!cancelQueue)
        {
            remainder--;

            if (remainder > 0)
            {
                Debug.Log("remaining: " + remainder);
                Invoke("timeStep", 1f);
            }
            else
            {
                Debug.Log("now block!");
                running = false;
                blocked = true;
                trigger();
            }
        }
        else
        {
            cancelQueue = running = false;
        }
    }

    IEnumerator changeMessage()
    {
        //yield return new WaitForSeconds(5); //TODO: delete
        //tmpMessage = textMessage.GetComponent<Text>().text;
        //int sum = BroadcastReceiver.estimatedFillStatus + BroadcastReceiver.fillStatusDifference;
        textMessage.GetComponent<Text>().fontSize = 36;
        textMessage.GetComponent<Text>().text = "Good job!";
        //textMessage.GetComponent<Text>().text = "Das letzte Mal haben sie am \n" + BroadcastReceiver.currentDrunkTime + "getrunken.\n" + 
        //  "Es waren " + sum.ToString() + " ml enthalten.\n Derzeit sind noch " + BroadcastReceiver.estimatedFillStatus.ToString() + " ml in Ihrem Becher. \nSie haben daher ungefähr " 
        //+ BroadcastReceiver.fillStatusDifference + " ml getrunken!";
        //Debug.Log(textMessage.GetComponent<Text>().text);
        yield return new WaitForSeconds(2);
        textMessage.GetComponent<Text>().fontSize = 36;
        textMessage.GetComponent<Text>().text = "Please drink something to continue!";
        unTrigger();
    }

}