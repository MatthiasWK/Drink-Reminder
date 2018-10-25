using System;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsManager : MonoBehaviour
{
    
    public Text DebugText;
    public ImageSettingsManager ImageScript;
    string ID;

    private void Start()
    {
        InitializePrefs();

        ImageScript.SetShapeTheme();
    }

    private void Update()
    {
        if (Constants.TriggerStop)
        {
            Constants.TriggerStop = false;
        }
        if (Constants.TriggerGo)
        {
            Constants.TriggerGo = false;
        }
    }

    private void OnEnable()
    {
        ID = GameController.tmp_Name;
        
        if (Constants.ShapesChanged)
        {
            ImageScript.LoadCustomShapes(ID);
        }
    }

    /// <summary>
    /// Load all recently set Prefs
    /// </summary>
    private void InitializePrefs()
    {
        if (GameController.login)
        {
            Constants.Rows = PlayerPrefs.GetInt(ID + "_Rows", 6);
            Constants.Columns = PlayerPrefs.GetInt(ID + "_Columns", 6);
            Constants.NumShapes = PlayerPrefs.GetInt(ID + "_NumShapes", 5);
            Constants.ShapeTheme = PlayerPrefs.GetInt(ID + "_ShapeTheme", 0);
            Constants.Background = PlayerPrefs.GetInt(ID + "_Background", 0);
            Constants.BackgroundPath = PlayerPrefs.GetString(ID + "_BackgroundPath", "Backgrounds");
            Constants.CustomBackgrounds = Convert.ToBoolean(PlayerPrefs.GetInt(ID + "_CustomBackgrounds", 0));
            Constants.CustomShapes = Convert.ToBoolean(PlayerPrefs.GetInt(ID + "_CustomShapes", 0));

            string[] customPaths = PlayerPrefsX.GetStringArray(ID + "_CustomPaths", "", 0);
            if (customPaths.Length > 0)
            {
                ImageScript.SetCustomPaths(customPaths);
            }
        }
        else
        {
            //todo: find better way to reset values
            Constants.Rows = 6;
            Constants.Columns = 6;
            Constants.NumShapes = 5;
            Constants.ShapeTheme = 0;
            Constants.Background = 0;
            Constants.BackgroundPath = "Backgrounds";
            Constants.CustomBackgrounds = false;
            Constants.CustomShapes = false;

            ImageScript.SetCustomPaths(new string[0]);
        }       

    }

    public void SetSmall()
    {
        Constants.Rows = 4;
        Constants.Columns = 4;
        Constants.NumShapes = 3;

        SavePrefs();
    }

    public void SetMedium()
    {
        Constants.Rows = 5;
        Constants.Columns = 5;
        Constants.NumShapes = 4;

        SavePrefs();
    }

    public void SetLarge()
    {
        Constants.Rows = 6;
        Constants.Columns = 6;
        Constants.NumShapes = 5;

        SavePrefs();
    }

    private void SavePrefs()
    {
        if (GameController.login)
        {
            PlayerPrefs.SetInt(ID + "_Rows", Constants.Rows);
            PlayerPrefs.SetInt(ID + "_Columns", Constants.Columns);
            PlayerPrefs.SetInt(ID + "_NumShapes", Constants.NumShapes);
        }
    }
}
