using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageSettingsManager : MonoBehaviour {

    //public Image TestImage;
    public Text DebugText;
    
    public Text Name;
    public Text Back;

    public Button StartButton;
    public Toggle BackgroundToggle;
    public Toggle ShapeToggle;

    public GameObject[] Shapes;
    public GameObject[] CustomShapes;
    public GameObject ShapesContainer;
    public GameObject CustomShapesContainer;
    //public GameObject[] BonusPrefabs;

    public GameObject Background;
    public string[] BackgroundFolders;
    
     string ID;

    private void Start()
    {
        ID = GameController.tmp_Name;
        Back.text = Constants.BackgroundPath;
        SetShapeTheme();

        BackgroundToggle.isOn = Constants.CustomBackgrounds;
        ShapeToggle.isOn = Constants.CustomShapes;
        ToggleCustomShapes(Constants.CustomShapes);
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

        LoadCustomShapes(ID);

        CheckPlayable();
    }

    public void LoadCustomShapes(string player)
    {
        for (int t = 0; t < CustomShapes.Length; t++)
        {
            Texture2D texture = NativeGallery.LoadImageAtPath(Application.persistentDataPath + "/" + player + "_shape_" + t + ".png");
            if (texture == null)
            {
                print("Couldn't load texture from " + Application.persistentDataPath + "/" + player + "_shape_" + t + ".png");
                CustomShapes[t].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("DefaultShapes/shape_" + t);
            }
            else
            {
                print("Loaded texture from: " + Application.persistentDataPath + "/" + player + "_shape_" + t + ".png");
                Sprite mySprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                CustomShapes[t].GetComponent<SpriteRenderer>().sprite = mySprite;
            }
        }
    }

    public void ChangeShapeTheme(int Change)
    {
        Constants.ShapeTheme = (Constants.ShapeTheme + Change) % 3;

        SetShapeTheme();

        if (GameController.login)
            PlayerPrefs.SetInt(ID + "_ShapeTheme", Constants.ShapeTheme);
    }

    public void SetShapeTheme()
    {
        if (Constants.ShapeTheme == 0)
        {
            Name.text = "Faces";

            Shapes[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fam_blue");
            Shapes[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fam_green");
            Shapes[2].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fam_orange");
            Shapes[3].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fam_red");
            Shapes[4].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fam_yellow");
        }
        else if (Constants.ShapeTheme == 1)
        {
            Name.text = "Candy";

            Shapes[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/bean_blue");
            Shapes[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/bean_green");
            Shapes[2].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/bean_orange");
            Shapes[3].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/bean_red");
            Shapes[4].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/bean_yellow");
        }
        else if (Constants.ShapeTheme == 2)
        {
            Name.text = "Animals";

            Shapes[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Dog");
            Shapes[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Frog");
            Shapes[2].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Pig");
            Shapes[3].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Duck");
            Shapes[4].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Chicken");
        }
    }

    public void ChangeBackground(int Change)
    {
        Constants.Background = (Constants.Background + Change) % BackgroundFolders.Length;

        Constants.BackgroundPath = BackgroundFolders[Constants.Background];

        Back.text = Constants.BackgroundPath;

        Constants.BackgroundsChanged = true;
        if (GameController.login)
        {
            PlayerPrefs.SetInt(ID + "_Background", Constants.Background);
            PlayerPrefs.SetString(ID + "_BackgroundPath", Constants.BackgroundPath);
        }
            
    }

    public void ToggleCustomBackgrounds(bool c)
    {
        Constants.CustomBackgrounds = c;
        Constants.BackgroundsChanged = true;
        CheckPlayable();

        if (GameController.login)
            PlayerPrefs.SetInt(ID + "_CustomBackgrounds", Convert.ToInt32(Constants.CustomBackgrounds));
    }

    public void ToggleCustomShapes(bool c)
    {
        Constants.CustomShapes = c;

        if (c)
        {
            CustomShapesContainer.SetActive(true);
            ShapesContainer.SetActive(false);
        }
        else
        {
            CustomShapesContainer.SetActive(false);
            ShapesContainer.SetActive(true);
        }

        if (GameController.login)
            PlayerPrefs.SetInt(ID + "_CustomShapes", Convert.ToInt32(Constants.CustomShapes));
    }

    public  void SetCustomPaths(String[] paths)
    {
        Background.GetComponent<BackgroundSpriteController>().paths = paths;
    }

    private void CheckPlayable()
    {
        if (Constants.CustomBackgrounds && Background.GetComponent<BackgroundSpriteController>().paths.Length == 0)
        {
            StartButton.interactable = false;
        }
        else
        {
            StartButton.interactable = true;
        }
    }

    public void PickImagePaths()
    {
        NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery((paths) =>
        {
            Debug.Log("Image path: " + paths);
            if (paths != null)
            {
                SetCustomPaths(paths);

                PlayerPrefsX.SetStringArray("CustomPaths", paths);
            }
        }, "Select a PNG image", "image/png");

        DebugText.text = "Permission result: " + permission;

        Constants.BackgroundsChanged = true;
        CheckPlayable();
    }
}
