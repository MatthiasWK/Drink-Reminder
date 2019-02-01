using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageSettingsManager : MonoBehaviour {

    //public Image TestImage;
    public Text DebugText;

    public Button StartButton;

    public GameObject[] Shapes;
    public GameObject[] CustomShapes;
    public GameObject ShapesContainer;
    public GameObject CustomShapesContainer;
    public Dropdown ShapesDropdown;

    public GameObject Background;
    public string[] BackgroundFolders;
    public Dropdown BGDropdown;
    
     string ID = null;

    private void Start()
    {
        //ID = GameController.tmp_Name;
        //ChangeShapeTheme(Constants.ShapeTheme);

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
        if (ID == null)
            ID = GameController.tmp_Name;

        LoadCustomShapes(ID);

        CheckPlayable();
    }

    /// <summary>
    /// Load all previously set custom shapes
    /// </summary>
    /// <param name="player"></param>
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

    /// <summary>
    /// Change the set of Shapes used in the game, called when selecting with the dropdown menu
    /// </summary>
    /// <param name="i"></param>
    public void ChangeShapeTheme(int i)
    {
        Constants.ShapeTheme = i;

        if (GameController.login)
            PlayerPrefs.SetInt(ID + "_ShapeTheme", Constants.ShapeTheme);

        if (i == 0)
        {

            Shapes[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fish_blue");
            Shapes[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fish_green");
            Shapes[2].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fish_orange");
            Shapes[3].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fish_red");
            Shapes[4].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fish_pink");

            var s = Shapes[0].GetComponent<SpriteRenderer>().sprite.texture.height;
            foreach(GameObject go in Shapes)
                go.transform.localScale = new Vector2(4000f / s, 4000f / s);
        }
        else if (i == 1)
        {

            Shapes[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Dog");
            Shapes[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Frog");
            Shapes[2].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Pig");
            Shapes[3].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Duck");
            Shapes[4].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Chicken");

            var s = Shapes[0].GetComponent<SpriteRenderer>().sprite.texture.height;
            foreach (GameObject go in Shapes)
                go.transform.localScale = new Vector2(4000f / s, 4000f / s);
        }
        else if (i == 2)
        {

            Shapes[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/gem_blue");
            Shapes[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/gem_green");
            Shapes[2].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/gem_orange");
            Shapes[3].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/gem_yellow");
            Shapes[4].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/gem_red");

            var s = Shapes[0].GetComponent<SpriteRenderer>().sprite.texture.height;
            foreach (GameObject go in Shapes)
                go.transform.localScale = new Vector2(4000f / s, 4000f / s);
        }
        else if (i == 3)
        {

            Shapes[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Vegetable_red");
            Shapes[2].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Vegetable_yellow");
            Shapes[3].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Vegetable_purple");
            Shapes[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Vegetable_beige");
            Shapes[4].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/Vegetable_green");

            var s = Shapes[0].GetComponent<SpriteRenderer>().sprite.texture.height;
            foreach (GameObject go in Shapes)
                go.transform.localScale = new Vector2(4000f / s, 4000f / s);
        }
        else if(i == 4)
        {
            ToggleCustomShapes(true);
            return;
        }

        if (Constants.CustomShapes)
            ToggleCustomShapes(false);
    }

    /// <summary>
    /// Switches to the next (c = 1) or last (c = -1) item in the dropdown list
    /// </summary>
    /// <param name="c"></param>
    public void CycleShapeTheme(int c)
    {
        c += Constants.ShapeTheme;
        int i = 0;

        if (c < 0)
            i = ShapesDropdown.options.Count - 1;
        else if (c > ShapesDropdown.options.Count - 1)
            i = 0;
        else
            i = c;

        ChangeShapeTheme(i);
        SetShapeTheme(i);

    }

    /// <summary>
    /// Sets the value of the dropdown menu, called within a script
    /// </summary>
    /// <param name="i"></param>
    public void SetShapeTheme(int i)
    {
        ShapesDropdown.value = i;
    }

    /// <summary>
    /// Enables/Disables the shape containers and sets the 'CustomShapes' variable
    /// </summary>
    /// <param name="c"></param>
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

    /// <summary>
    /// Change the set of Backgrounds used in the game, called when selecting with the dropdown menu
    /// </summary>
    /// <param name="i"></param>
    public void ChangeBackground(int i)
    {
        Constants.Background = i;

        if (i < BackgroundFolders.Length)
        {
            Constants.BackgroundPath = BackgroundFolders[Constants.Background];
            Constants.CustomBackgrounds = false;
        }
        else
        {
            Constants.CustomBackgrounds = true;           
        }

        //Back.text = Constants.BackgroundPath;
        CheckPlayable();
        Constants.BackgroundsChanged = true;

        if (GameController.login)
        {
            PlayerPrefs.SetInt(ID + "_Background", Constants.Background);
            PlayerPrefs.SetString(ID + "_BackgroundPath", Constants.BackgroundPath);
            PlayerPrefs.SetInt(ID + "_CustomBackgrounds", Convert.ToInt32(Constants.CustomBackgrounds));
        }           
    }

    /// <summary>
    /// Switches to the next (c = 1) or last (c = -1) item in the dropdown list
    /// </summary>
    /// <param name="c"></param>
    public void CycleBackground(int c)
    {
        c += Constants.Background;
        int i = 0;

        if (c < 0)
            i = BackgroundFolders.Length;
        else if (c > BackgroundFolders.Length)
            i = 0;
        else
            i = c;

        ChangeBackground(i);
        SetBackground(i);
    }

    /// <summary>
    /// Sets the value of the dropdown menu, called within a script
    /// </summary>
    /// <param name="i"></param>
    public void SetBackground(int i)
    {
        BGDropdown.value = i;
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

    /// <summary>
    /// Launches the mobile devices native gallery so the user can pick 1 or more images.
    /// The file paths of these images are then saved and later loaded onto the GameBackground gameobject.
    /// Also sets the Dropdown menu to 'Custom' if successfully picked.
    /// </summary>
    public void PickImagePaths()
    {
        NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery((paths) =>
        {
            Debug.Log("Image path: " + paths);
            if (paths != null)
            {
                SetCustomPaths(paths);

                PlayerPrefsX.SetStringArray(ID + "_CustomPaths", paths);

                ChangeBackground(BackgroundFolders.Length);
                SetBackground(BackgroundFolders.Length);
            }
        }, "Select a PNG image", "image/png");

        //DebugText.text = "Permission result: " + permission;

        Constants.BackgroundsChanged = true;
        CheckPlayable();
    }
}
