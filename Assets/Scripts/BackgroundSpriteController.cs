using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundSpriteController : MonoBehaviour {

    public Text DebugText;

    public Sprite[] Sprites;
    public Sprite[] CustomSprites;

    public string[] paths;

    public Canvas Error;
    public RectTransform GameCanvas;

    private Sprite[] Backgrounds;

    private System.Random rnd = new System.Random();

    int i = 0;

    private void OnEnable()
    {
        if (Variables.BackgroundsChanged)
        {
            LoadSprites();
        }
        else
        {
            NextSprite();
        }
    }

    /// <summary>
    /// Loads an array of sprites. Either by importing images from device storage or the Resources folder.
    /// </summary>
    private void LoadSprites()
    {
        if (Variables.CustomBackgrounds)
        {

            List<Sprite> NewSprites = new List<Sprite>();

            for (int i = 0; i < paths.Length; i++)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(paths[i]);
                if (texture == null)
                {
                    //DebugText.text = "Couldn't load texture from " + paths[i];
                    Error.gameObject.SetActive(true);
                    return;
                }
                else
                {
                    Sprite mySprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

                    NewSprites.Add(mySprite);
                }
            }

            CustomSprites = NewSprites.ToArray();

            Backgrounds = CustomSprites;
        }
        else
        {
            Sprites = Resources.LoadAll<Sprite>(Variables.BackgroundPath);
            Backgrounds = Sprites;
        }

        Variables.BackgroundsChanged = false;


        Shuffle();
        SetSprite(0);
    }

    public void NextSprite()
    {
        if(Backgrounds != null)
        {
            i = (i +1) % Backgrounds.Length;
            SetSprite(i);
        }
    }

    private void SetSprite(int s)
    {
        GetComponent<SpriteRenderer>().sprite = Backgrounds[s];

        ResizeSpriteToScreen();
    }

    public void SetCustomSprites(Sprite[] NewSprites)
    {
        CustomSprites = NewSprites;
    }

    /// <summary>
    /// Resizes a Sprite fit the screen, either to its width (fitToScreenWidthHeight = 0) or height (fitToScreenWidthHeight = 1)
    /// </summary>
    /// <param name="theSprite"></param>
    /// <param name="theCamera"></param>
    private void ResizeSpriteToScreen()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        transform.localScale = new Vector3(1, 1, 1);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = (float)(Camera.main.orthographicSize * 2.0);
        float worldScreenWidth = (float)(worldScreenHeight / Screen.height * Screen.width);

        if (height > width)
        {
            Vector2 sizeX = new Vector2(worldScreenHeight / height, worldScreenHeight / height);
            transform.localScale = sizeX;

            transform.SetPositionAndRotation(GameCanvas.position, Quaternion.identity);
        }
        else if (width > height)
        {
            Vector2 sizeY = new Vector2(worldScreenWidth / width, worldScreenWidth / width);
            transform.localScale = sizeY;

            // move to left side of screen
            float screenLeft = worldScreenWidth * -0.5f;
            float newWidth = width * (worldScreenWidth / width);
            transform.SetPositionAndRotation(new Vector3((newWidth * 0.5f) + screenLeft, 0, 0), Quaternion.identity);
        }
        int i = 0;
    }

    /// <summary>
    /// Shuffle the array.
    /// </summary>
    private void Shuffle()
    {
        int n = Backgrounds.Length;
        for (int i = 0; i < n; i++)
        {
            int r = i + rnd.Next(n - i);
            Sprite t = Backgrounds[r];
            Backgrounds[r] = Backgrounds[i];
            Backgrounds[i] = t;
        }
    }

    public void ClearCustom()
    {
        CustomSprites = new Sprite[0];
        paths = new string[0];
        Variables.BackgroundsChanged = true;

        Error.gameObject.SetActive(false);
    }
}
