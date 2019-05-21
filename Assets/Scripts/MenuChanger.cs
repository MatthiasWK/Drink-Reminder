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

    /// <summary>
    /// Game Settings Menu is always loaded first
    /// </summary>
    private void Start()
    {
        if (ID == null)
            ID = GameController.tmp_Name;

        LoadMenu();

    }

    /// <summary>
    /// Determines effects of Device's Back Buttons
    /// </summary>
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

    /// <summary>
    /// Activate Game Canvas
    /// </summary>
    public void LoadGame()
    {
        Game.gameObject.SetActive(true);
        Background.SetActive(true);
        Menu.gameObject.SetActive(false);     

        Companion.StartSayGame();

        Variables.IsPlaying = true;
    }

    /// <summary>
    /// Activate Game Settings Canvas
    /// </summary>
    public void LoadMenu()
    {
        Game.gameObject.SetActive(false);
        Background.SetActive(false);
        Menu.gameObject.SetActive(true);
        ImageSettings.gameObject.SetActive(false);
        CustomizationCanvas.gameObject.SetActive(false);

        Companion.StartSayGameMenu();

        Variables.IsPlaying = false;
    }

    /// <summary>
    /// Activate Image Settings Canvas
    /// </summary>
    public void LoadImageSettings()
    {
        Menu.gameObject.SetActive(false);
        ImageSettings.gameObject.SetActive(true);
        CustomizationCanvas.gameObject.SetActive(false);

        Companion.StartSaySettings();
    }

    /// <summary>
    /// Activate Customization Canvas
    /// </summary>
    public void LoadCustomization()
    {
        ImageSettings.gameObject.SetActive(false);
        CustomizationCanvas.gameObject.SetActive(true);

        Companion.StartSayCustomization();
    }

    /// <summary>
    /// Load different Scene
    /// </summary>
    public void Load(string name)
    {       
         SceneManager.LoadScene(name);        
    }

}
