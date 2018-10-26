using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public void Load(string name)
    {

        SceneManager.LoadScene(name);

    }

    public static void LoadInScript(string name)
    {

        SceneManager.LoadScene(name);

    }

    public void LogOut()
    {
        Constants.BackgroundsChanged = true;
        GameController.login = false;
        SimpleTest.previousWeight = 0.0f;
        SimpleTest.updateUser = true;
    }

    public void ExitGame()
    {
        LogOut();
        SceneManager.LoadScene("Menue");
    }
}
