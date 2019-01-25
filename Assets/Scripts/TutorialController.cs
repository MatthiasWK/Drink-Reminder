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
            FirstTimeTuorial();
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
        string[] arr = { "Test_A" };

        LoadTutorial(arr);
    }

    public void GameTuorial()
    {
        string[] arr = { "Test_A" };

        LoadTutorial(arr);
    }

    public void SettingsTuorial()
    {
        string[] arr = { "Test_A" };

        LoadTutorial(arr);
    }

    public void CustomTuorial()
    {
        string[] arr = { "Test_A" };

        LoadTutorial(arr);
    }

    public void TestTuorial()
    {
        string[] arr = { "Test_A" };

        LoadTutorial(arr);
    }

    public void FirstTimeTuorial()
    {
        string[] arr = { "Test_A" };

        LoadTutorial(arr);
    }
}
