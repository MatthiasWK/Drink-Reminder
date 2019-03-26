using System;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsManager : MonoBehaviour
{
    
    //public Text DebugText;
    public ImageSettingsManager ImageScript;
    public Toggle Toggle_s;
    public Toggle Toggle_m;
    public Toggle Toggle_l;
    public Text Size_Description;
    public Text Mode_Description;
    string ID = null;

    private void Start()
    {
        InitializePrefs();

        ImageScript.ChangeShapeTheme(Constants.ShapeTheme);
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
        if(ID == null)
            ID = GameController.tmp_Name;
        
        if (Constants.ShapesChanged)
            ImageScript.LoadCustomShapes(ID);
    }

    /// <summary>
    /// Load all recently set Prefs
    /// </summary>
    private void InitializePrefs()
    {
        if (GameController.login)
        {
            //Constants.Rows = PlayerPrefs.GetInt(ID + "_Rows", 6);
            //Constants.Columns = PlayerPrefs.GetInt(ID + "_Columns", 6);
            //Constants.NumShapes = PlayerPrefs.GetInt(ID + "_NumShapes", 5);
            Constants.GameSize = PlayerPrefs.GetInt(ID + "_GameSize", 0);
            SetSize(Constants.GameSize);
            SetToggle(Constants.GameSize);
            Constants.ShapeTheme = PlayerPrefs.GetInt(ID + "_ShapeTheme", 0);
            ImageScript.SetShapeTheme(Constants.ShapeTheme);
            Constants.Background = PlayerPrefs.GetInt(ID + "_Background", 0);
            ImageScript.SetBackground(Constants.Background);
            Constants.BackgroundPath = PlayerPrefs.GetString(ID + "_BackgroundPath", "Backgrounds");
            Constants.CustomBackgrounds = Convert.ToBoolean(PlayerPrefs.GetInt(ID + "_CustomBackgrounds", 0));
            Constants.CustomShapes = Convert.ToBoolean(PlayerPrefs.GetInt(ID + "_CustomShapes", 0));

            if (Constants.CustomShapes)
                ImageScript.SetPreviewSprite("Backgrounds/preview");
            else
                ImageScript.SetPreviewSprite(Constants.BackgroundPath + "/preview");

            string[] customPaths = PlayerPrefsX.GetStringArray(ID + "_CustomPaths", "", 0);

            if (customPaths.Length > 0)
                ImageScript.SetCustomPaths(customPaths);
        }
        else
        {
            //todo: find better way to reset values
            //Constants.Rows = 6;
            //Constants.Columns = 6;
            //Constants.NumShapes = 5;
            Constants.GameSize = 0;
            SetSize(0);
            SetToggle(0);
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

        Size_Description.text = "Kleines Spiel\n4 x 4 Reihen\n3 verschiedene Objekte";

        SavePrefs();
    }

    public void SetMedium()
    {
        Constants.Rows = 5;
        Constants.Columns = 5;
        Constants.NumShapes = 4;

        Size_Description.text = "Mittelgroßes Spiel\n5 x 5 Reihen\n4 verschiedene Objekte";

        SavePrefs();
    }

    public void SetLarge()
    {
        Constants.Rows = 6;
        Constants.Columns = 6;
        Constants.NumShapes = 5;

        Size_Description.text = "Großes Spiel\n6 x 6 Reihen\n5 verschiedene Objekte";

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

    public void SetGameMode(int m)
    {
        Constants.GameMode = m;

        if (m == 0)
            Mode_Description.text = "Bilde Reihen um Punkte zu bekommen und ein Bild zu enthüllen!";
        if (m == 1)
            Mode_Description.text = "Bilde Reihen um Kacheln zu entfernen hinter denen sich ein Bild versteckt!";
    }

    public void SetSize(int s)
    {
        Constants.GameSize = s;

        Constants.Rows = 4 + s;
        Constants.Columns = 4 + s;
        Constants.NumShapes = 3 + s;

        if (s == 0)
            Size_Description.text = "Kleines Spiel\n4 x 4 Felder\n3 verschiedene Objekte";
        if (s == 1)
            Size_Description.text = "Mittelgroßes Spiel\n5 x 5 Felder\n4 verschiedene Objekte";
        if (s == 2)
            Size_Description.text = "Großes Spiel\n6 x 6 Felder\n5 verschiedene Objekte";

        if (GameController.login)
            PlayerPrefs.SetInt(ID + "_GameSize", s);
    }

    private void SetToggle(int s)
    {
        if (s == 0)
            Toggle_s.isOn = true;
        else if (s == 1)
            Toggle_m.isOn = true;
        else if (s == 2)
            Toggle_l.isOn = true;
    }
}
