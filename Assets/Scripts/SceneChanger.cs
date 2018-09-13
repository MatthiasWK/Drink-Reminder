using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour {

    public Canvas Game;
    public Canvas Menu;
    public GameObject Background;

    private void Start()
    {
        LoadMenu();
    }

    public void LoadGame()
    {
        Game.gameObject.SetActive(true);
        Background.SetActive(true);
        Menu.gameObject.SetActive(false);
    }

    public void LoadMenu()
    {
        Game.gameObject.SetActive(false);
        Background.SetActive(false);
        Menu.gameObject.SetActive(true);
    }

    public void Load(string name)
    {
        
         SceneManager.LoadScene(name);
        
    }

}
