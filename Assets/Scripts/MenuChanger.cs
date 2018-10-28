using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuChanger : MonoBehaviour {

    public Canvas Game;
    public Canvas Menu;
    public Canvas ImageSettings;
    public Canvas CustomizationCanvas;

    private void Start()
    {
        LoadMenu();
    }

    public void LoadGame()
    {
        Game.gameObject.SetActive(true);
        Menu.gameObject.SetActive(false);
    }

    public void LoadMenu()
    {
        Game.gameObject.SetActive(false);
        Menu.gameObject.SetActive(true);
        ImageSettings.gameObject.SetActive(false);
        CustomizationCanvas.gameObject.SetActive(false);
    }

    public void LoadImageSettings()
    {
        Menu.gameObject.SetActive(false);
        ImageSettings.gameObject.SetActive(true);
        CustomizationCanvas.gameObject.SetActive(false);
    }

    public void LoadCustomization()
    {
        ImageSettings.gameObject.SetActive(false);
        CustomizationCanvas.gameObject.SetActive(true);
    }
    public void Load(string name)
    {
        
         SceneManager.LoadScene(name);
        
    }

}
