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
    public Toggle Toggle_p;
    public Toggle Toggle_t;
    public Text Size_Description;
    public Text Mode_Description;
    string ID = null;

    private void Start()
    {
        InitializePrefs();

        ImageScript.ChangeShapeTheme(Variables.ShapeTheme);
    }

    private void Update()
    {
        if (Variables.TriggerStop)
        {
            Variables.TriggerStop = false;
        }
        if (Variables.TriggerGo)
        {
            Variables.TriggerGo = false;
        }
    }

    private void OnEnable()
    {
        if(ID == null)
            ID = GameController.tmp_Name;
        
        if (Variables.ShapesChanged)
            ImageScript.LoadCustomShapes(ID);
    }

    /// <summary>
    /// Load all recently set Prefs
    /// </summary>
    private void InitializePrefs()
    {
        if (GameController.login)
        {
            Variables.GameSize = PlayerPrefs.GetInt(ID + "_GameSize", 0);
            SetSize(Variables.GameSize);
            SetSizeToggle(Variables.GameSize);
            Variables.GameMode = PlayerPrefs.GetInt(ID + "_GameMode", 0);
            SetGameMode(Variables.GameMode);
            SetModeToggle(Variables.GameMode);
            Variables.ShapeTheme = PlayerPrefs.GetInt(ID + "_ShapeTheme", 0);
            ImageScript.SetShapeTheme(Variables.ShapeTheme);
            Variables.Background = PlayerPrefs.GetInt(ID + "_Background", 0);
            ImageScript.SetBackground(Variables.Background);
            Variables.BackgroundPath = PlayerPrefs.GetString(ID + "_BackgroundPath", "Backgrounds");
            Variables.CustomBackgrounds = Convert.ToBoolean(PlayerPrefs.GetInt(ID + "_CustomBackgrounds", 0));
            Variables.CustomShapes = Convert.ToBoolean(PlayerPrefs.GetInt(ID + "_CustomShapes", 0));

            if (Variables.CustomShapes)
                ImageScript.SetPreviewSprite("Backgrounds/preview");
            else
                ImageScript.SetPreviewSprite(Variables.BackgroundPath + "/preview");

            string[] customPaths = PlayerPrefsX.GetStringArray(ID + "_CustomPaths", "", 0);

            if (customPaths.Length > 0)
                ImageScript.SetCustomPaths(customPaths);
        }
        else
        {
            Variables.GameSize = 0;
            SetSize(0);
            SetSizeToggle(0);
            SetGameMode(0);
            SetModeToggle(0);
            Variables.ShapeTheme = 0;
            Variables.Background = 0;
            Variables.BackgroundPath = "Backgrounds";
            Variables.CustomBackgrounds = false;
            Variables.CustomShapes = false;

            ImageScript.SetCustomPaths(new string[0]);
        }       
    }

    public void SetGameMode(int m)
    {
        Variables.GameMode = m;
        PlayerPrefs.SetInt(ID + "_GameMode", m);

        if (m == 0)
            Mode_Description.text = "Bilde Reihen um Punkte zu bekommen und ein Bild zu enthüllen!";
        if (m == 1)
            Mode_Description.text = "Bilde Reihen um Kacheln zu entfernen hinter denen sich ein Bild versteckt!";
    }

    private void SetModeToggle(int m)
    {
        if (m == 0)
            Toggle_p.isOn = true;
        else if (m == 1)
            Toggle_t.isOn = true;
    }

    public void SetSize(int s)
    {
        Variables.GameSize = s;

        Variables.Rows = 4 + s;
        Variables.Columns = 4 + s;
        Variables.NumShapes = 3 + s;

        if (s == 0)
            Size_Description.text = "Kleines Spiel\n4 x 4 Felder\n3 verschiedene Objekte";
        if (s == 1)
            Size_Description.text = "Mittelgroßes Spiel\n5 x 5 Felder\n4 verschiedene Objekte";
        if (s == 2)
            Size_Description.text = "Großes Spiel\n6 x 6 Felder\n5 verschiedene Objekte";

        if (GameController.login)
            PlayerPrefs.SetInt(ID + "_GameSize", s);
    }

    private void SetSizeToggle(int s)
    {
        if (s == 0)
            Toggle_s.isOn = true;
        else if (s == 1)
            Toggle_m.isOn = true;
        else if (s == 2)
            Toggle_l.isOn = true;
    }
}
