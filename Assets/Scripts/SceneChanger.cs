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

    public static void LogOut()
    {
        Variables.BackgroundsChanged = true;
        GameController.login = false;
        //SimpleTest.previousWeight = 0.0f;
        //SimpleTest.updateUser = true;
    }

    public void ExitGame()
    {
        LogOut();
        SceneManager.LoadScene("Menue");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == "Menue")
        {
            Application.Quit();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && (SceneManager.GetActiveScene().name == "CreateProfile_EnterName" || SceneManager.GetActiveScene().name == "LoadProfile"))
        {
            SceneManager.LoadScene("Menue");
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == "CreateProfile_EnterGender")
        {
            SceneManager.LoadScene("CreateProfile_EnterName");
        }
    }

}
