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
    public Toggle CustomToggle;

    public GameObject[] ShapePrefabs;
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

        CustomToggle.isOn = Constants.Custom;
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
    }

    private void InitializePrefs()
    {
        Constants.Rows = PlayerPrefs.GetInt("Rows", Constants.Rows);
        Constants.Columns = PlayerPrefs.GetInt("Columns", Constants.Columns);
        Constants.NumShapes = PlayerPrefs.GetInt("NumShapes", Constants.NumShapes);
        Constants.ShapeTheme = PlayerPrefs.GetInt("ShapeTheme", Constants.ShapeTheme);
        Constants.BackgroundPath = PlayerPrefs.GetString("BackgroundPath", Constants.BackgroundPath);
        Constants.Custom = Convert.ToBoolean(PlayerPrefs.GetInt("Custom", 0));

        string[] customPaths = PlayerPrefsX.GetStringArray("CustomPaths");
        if (customPaths.Length > 0)
        {
            Background.GetComponent<BackgroundSpriteController>().paths = customPaths;
        }

        CheckPlayable();

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

            ShapePrefabs[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fam_blue");
            ShapePrefabs[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fam_green");
            ShapePrefabs[2].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fam_orange");
            ShapePrefabs[3].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fam_red");
            ShapePrefabs[4].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fam_yellow");
        }
        else if (Constants.ShapeTheme == 1)
        {
            Name.text = "Candy";

            ShapePrefabs[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/bean_blue");
            ShapePrefabs[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/bean_green");
            ShapePrefabs[2].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/bean_orange");
            ShapePrefabs[3].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/bean_red");
            ShapePrefabs[4].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/bean_yellow");
        }
        else if (Constants.ShapeTheme == 2)
        {
            Name.text = "Animals";

            ShapePrefabs[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Dog");
            ShapePrefabs[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Frog");
            ShapePrefabs[2].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Pig");
            ShapePrefabs[3].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Duck");
            ShapePrefabs[4].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Chicken");
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

    public void ToggleCustom(bool c)
    {
        Constants.Custom = c;
        Constants.BackgroundsChanged = true;
        CheckPlayable();

        PlayerPrefs.SetInt("Custom", Convert.ToInt32(Constants.Custom));
    }

    private void CheckPlayable()
    {
        if (Constants.Custom && Background.GetComponent<BackgroundSpriteController>().paths.Length == 0)
        {
            StartButton.interactable = false;
        }
        else
        {
            StartButton.interactable = true;
        }
    }

    public void PickImage()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                //DebugText.text = path;
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                
                string[] NewSprites = new string[1];
                NewSprites[0] = path;
                Background.GetComponent<BackgroundSpriteController>().paths = NewSprites;
            }
        }, "Select a PNG image", "image/png");

        Debug.Log("Permission result: " + permission);
        Constants.BackgroundsChanged = true;
        CheckPlayable();
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
