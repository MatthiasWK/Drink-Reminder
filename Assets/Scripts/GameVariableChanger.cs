using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameVariableChanger : MonoBehaviour {
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

        Size.text = Constants.Rows + " x " + Constants.Columns;
        Num.text = Constants.NumShapes.ToString();
        Back.text = Constants.Path;
        SetTheme();

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
    }

    public void ChangeTheme(int Change)
    {
        Constants.Theme = (Constants.Theme + Change) % 3;

        SetTheme();
        
    }

    private void SetTheme()
    {
        if (Constants.Theme == 0)
        {
            Name.text = "Faces";

            ShapePrefabs[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fam_blue");
            ShapePrefabs[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fam_green");
            ShapePrefabs[2].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fam_orange");
            ShapePrefabs[3].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fam_red");
            ShapePrefabs[4].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/fam_yellow");
        }
        else if (Constants.Theme == 1)
        {
            Name.text = "Candy";

            ShapePrefabs[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/bean_blue");
            ShapePrefabs[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/bean_green");
            ShapePrefabs[2].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/bean_orange");
            ShapePrefabs[3].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/bean_red");
            ShapePrefabs[4].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Shapes/bean_yellow");
        }
        else if (Constants.Theme == 2)
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

        Constants.Path = BackgroundFolders[Constants.Background];

        Back.text = Constants.Path;

        Constants.BackgroundsChanged = true;
    }

    public void ToggleCustom(bool c)
    {
        Constants.Custom = c;
        Constants.BackgroundsChanged = true;
        CheckPlayable();
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
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                Sprite mySprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                
            }
        }, "Select a PNG image", "image/png");

        Debug.Log("Permission result: " + permission);
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
            }
        }, "Select a PNG image", "image/png");

        Debug.Log("Permission result: " + permission);

        CheckPlayable();
    }
}
