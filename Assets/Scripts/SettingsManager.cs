using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {

    //public Image TestImage;
    public Text DebugText;

    public Text Countdown;

    public Text Size;
    public Text Num;
    public Text Name;
    public Text Back;

    public Canvas DrinkCanvas;

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

    private bool counting = true;

    private void Start()
    {
        InitializePrefs();

        Size.text = Constants.Rows + " x " + Constants.Columns;
        Num.text = Constants.NumShapes.ToString();
        Back.text = Constants.BackgroundPath;
        SetShapeTheme();

        BackgroundToggle.isOn = Constants.CustomBackgrounds;
        ShapeToggle.isOn = Constants.CustomShapes;
        ToggleCustomShapes(Constants.CustomShapes);
    }

    private void Update()
    {
        if (counting)
        {
            if (Constants.TimeLeft > 0)
            {
                Constants.TimeLeft -= Time.deltaTime;
                Countdown.text = Constants.TimeLeft.ToString("00");
            }
            else
            {
                DrinkCanvas.gameObject.SetActive(true);
                counting = false;
            }
        }
    }

    private void OnEnable()
    {
        CheckPlayable();

        if (Constants.ShapesChanged)
        {
            LoadCustomShapes();
        }
    }

    private void InitializePrefs()
    {
        Constants.Rows = PlayerPrefs.GetInt("Rows", Constants.Rows);
        Constants.Columns = PlayerPrefs.GetInt("Columns", Constants.Columns);
        Constants.NumShapes = PlayerPrefs.GetInt("NumShapes", Constants.NumShapes);
        Constants.ShapeTheme = PlayerPrefs.GetInt("ShapeTheme", Constants.ShapeTheme);
        Constants.BackgroundPath = PlayerPrefs.GetString("BackgroundPath", Constants.BackgroundPath);
        Constants.CustomBackgrounds = Convert.ToBoolean(PlayerPrefs.GetInt("CustomBackgrounds", 0));
        Constants.CustomShapes = Convert.ToBoolean(PlayerPrefs.GetInt("CustomShapes", 0));

        string[] customPaths = PlayerPrefsX.GetStringArray("CustomPaths");
        if (customPaths.Length > 0)
        {
            Background.GetComponent<BackgroundSpriteController>().paths = customPaths;
        }

        CheckPlayable();

    }

    private void LoadCustomShapes()
    {
        for (int t = 0; t < CustomShapes.Length; t++)
        {
            Texture2D texture = NativeGallery.LoadImageAtPath(Application.persistentDataPath + "/" + "shape_" + t + ".png");
            if (texture == null)
            {
                print("Couldn't load texture from " + Application.persistentDataPath + "/" + "shape_" + t + ".png");
            }
            else
            {
                Sprite mySprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                CustomShapes[t].GetComponent<SpriteRenderer>().sprite = mySprite;
            }
        }
    }

    public void Drink()
    {
        if (isActiveAndEnabled)
        {
            DrinkCanvas.gameObject.SetActive(false);
            counting = true;
            Constants.TimeLeft = Constants.ReminderTime;
        }
    }

    public void ChangeGridSize(int Change)
    {
        
        if (Change == 0 && Constants.Rows > 3)
        {
            Constants.Rows -= 1;
            Constants.Columns -= 1;
        }
        else if (Change == 1 && Constants.Rows < 10)
        {
            Constants.Rows += 1;
            Constants.Columns += 1;
        }

        Size.text = Constants.Rows + " x " + Constants.Columns;

        PlayerPrefs.SetInt("Rows", Constants.Rows);
        PlayerPrefs.SetInt("Columns", Constants.Columns);
    }

    public void ChangeShapeNum(int Change)
    {

        if (Change == 0 && Constants.NumShapes > 2)
        {
            Constants.NumShapes -= 1;
        }
        else if (Change == 1 && Constants.NumShapes < 5)
        {
            Constants.NumShapes += 1;
        }

        Num.text = Constants.NumShapes.ToString();

        PlayerPrefs.SetInt("NumShapes", Constants.NumShapes);
    }

    public void ChangeShapeTheme(int Change)
    {
        Constants.ShapeTheme = (Constants.ShapeTheme + Change) % 3;

        SetShapeTheme();

        PlayerPrefs.SetInt("ShapeTheme", Constants.ShapeTheme);
    }

    private void SetShapeTheme()
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

        PlayerPrefs.SetString("BackgroundPath", Constants.BackgroundPath);
    }

    public void ToggleCustomBackgrounds(bool c)
    {
        Constants.CustomBackgrounds = c;
        Constants.BackgroundsChanged = true;
        CheckPlayable();

        PlayerPrefs.SetInt("CustomBackgrounds", Convert.ToInt32(Constants.CustomBackgrounds));
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

        PlayerPrefs.SetInt("CustomShapes", Convert.ToInt32(Constants.CustomShapes));
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

    public void PickImages()
    {
        NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery((paths) =>
        {
            Debug.Log("Image path: " + paths);
            if (paths != null)
            {
                Sprite[] NewSprites = new Sprite[paths.Length];
                for (int i = 0; i < paths.Length; i++)
                {
                    // Create Texture from selected image
                    Texture2D texture = NativeGallery.LoadImageAtPath(paths[i]);
                    if (texture == null)
                    {
                        Debug.Log("Couldn't load texture from " + paths[i]);
                        return;
                    }

                    Sprite mySprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

                    NewSprites[i] = mySprite;
                }
                Background.GetComponent<BackgroundSpriteController>().CustomSprites = NewSprites;
            }
        }, "Select a PNG image", "image/png");

        Debug.Log("Permission result: " + permission);

        CheckPlayable();
    }

    public void PickImagePaths()
    {
        NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery((paths) =>
        {
            Debug.Log("Image path: " + paths);
            if (paths != null)
            {
                Background.GetComponent<BackgroundSpriteController>().paths = paths;

                PlayerPrefsX.SetStringArray("CustomPaths", paths);
            }
        }, "Select a PNG image", "image/png");

        Debug.Log("Permission result: " + permission);

        Constants.BackgroundsChanged = true;
        CheckPlayable();
    }
}
