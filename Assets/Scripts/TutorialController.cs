using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {
    public Canvas TutorialCanvas;
    public Image CurrentSlide;
    public List<Sprite> TutorialSlides;
    public Button LoadButton;

    private void Start()
    {
        int first = PlayerPrefs.GetInt("FirstTime", 0);

        if(first == 0)
        {
            LoadButton.interactable = false;
            //FirstTimeTuorial();
            PlayerPrefs.SetInt("FirstTime", 1);
        }
    }

    private void LoadTutorial(string[] slides)
    {

        TutorialSlides = new List<Sprite>();

        for(int i = 0; i<slides.Length; i++)
        {
            TutorialSlides.Add(Resources.Load<Sprite>("Tutorials/" + slides[i]));
        }

        CurrentSlide.sprite = TutorialSlides[0];

        TutorialCanvas.gameObject.SetActive(true);
    }

    public void NextSlide()
    {
        TutorialSlides.RemoveAt(0);

        if (TutorialSlides.Count > 0)
        {
            CurrentSlide.sprite = TutorialSlides[0];
        }
        else
        {
            TutorialCanvas.gameObject.SetActive(false);
        }
    }

    public void GameMenuTuorial()
    {
        string[] arr = { "game_menu_1", "game_menu_2", "game_menu_3", "game_menu_4"};

        LoadTutorial(arr);
    }

    public void GameTuorial()
    {
        string[] arr = { "game_0", "game_1", "game_2", "game_3", "game_4", "game_5", "game_6", "game_7", "game_8", "game_9", "game_10", "game_11", "game_12", "game_13", "game_14", "game_15", "game_16", "game_17", "game_18" };

        LoadTutorial(arr);
    }

    public void SettingsTuorial()
    {
        string[] arr = { "settings_1", "settings_2", "settings_3", "settings_4", "settings_5", "settings_6"};

        LoadTutorial(arr);
    }

    public void CustomTuorial()
    {
        string[] arr = { "custom_1", "custom_2", "custom_3", "custom_4", "custom_5", "custom_6", "custom_7", "custom_8"};

        LoadTutorial(arr);
    }

    public void LoadTuorial()
    {
        string[] arr = { "load_1" };

        LoadTutorial(arr);
    }

    public void CreateTuorial()
    {
        string[] arr = { "create_1", "create_2" };

        LoadTutorial(arr);
    }

    public void FirstTimeTuorial()
    {
        string[] arr = { "menu_1", "menu_2", "menu_3", "menu_4", "menu_5" };

        LoadTutorial(arr);
    }
}
