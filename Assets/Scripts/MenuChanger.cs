using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuChanger : MonoBehaviour {

    public Canvas Game;
    public Canvas Menu;
    public Canvas ImageSettings;
    public Canvas CustomizationCanvas;
    public GameObject Background;
    public CompanionController Companion;
    public TutorialController Tutorial;
    string ID = null;

    private void Start()
    {
        if (ID == null)
            ID = GameController.tmp_Name;

        LoadMenu();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Menu.isActiveAndEnabled)
        {
            SceneChanger.LogOut();
            SceneManager.LoadScene("Menue");
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Game.isActiveAndEnabled)
        {
            LoadMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && ImageSettings.isActiveAndEnabled)
        {
            LoadMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && CustomizationCanvas.isActiveAndEnabled)
        {
            LoadImageSettings();
        }
    }

    public void LoadGame()
    {
        Game.gameObject.SetActive(true);
        Background.SetActive(true);
        Menu.gameObject.SetActive(false);

        //int t = PlayerPrefs.GetInt(ID + "_GameTutorial", 0);

        //if (t == 0)
        //{
        //    Tutorial.GameTuorial();
        //    PlayerPrefs.SetInt(ID + "_GameTutorial", 1);
        //}

        Companion.StartSayGame();
    }

    public void LoadMenu()
    {
        Game.gameObject.SetActive(false);
        Background.SetActive(false);
        Menu.gameObject.SetActive(true);
        ImageSettings.gameObject.SetActive(false);
        CustomizationCanvas.gameObject.SetActive(false);

        //int t = PlayerPrefs.GetInt(ID + "_GameMenuTutorial", 0);

        //if(t == 0)
        //{
        //    Tutorial.GameMenuTuorial();
        //    PlayerPrefs.SetInt(ID + "_GameMenuTutorial", 1);
        //}

        Companion.StartSayGameMenu();
    }

    public void LoadImageSettings()
    {
        Menu.gameObject.SetActive(false);
        ImageSettings.gameObject.SetActive(true);
        CustomizationCanvas.gameObject.SetActive(false);

        //int t = PlayerPrefs.GetInt(ID + "_SettingsTutorial", 0);

        //if (t == 0)
        //{
        //    Tutorial.SettingsTuorial();
        //    PlayerPrefs.SetInt(ID + "_SettingsTutorial", 1);
        //}

        Companion.StartSaySettings();
    }

    public void LoadCustomization()
    {
        ImageSettings.gameObject.SetActive(false);
        CustomizationCanvas.gameObject.SetActive(true);

        //int t = PlayerPrefs.GetInt(ID + "_CustomTutorial", 0);

        //if (t == 0)
        //{
        //    Tutorial.CustomTuorial();
        //    PlayerPrefs.SetInt(ID + "_CustomTutorial", 1);
        //}

        Companion.StartSayCustomization();
    }

    public void Load(string name)
    {       
         SceneManager.LoadScene(name);        
    }

}
