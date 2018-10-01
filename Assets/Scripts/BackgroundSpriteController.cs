using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpriteController : MonoBehaviour {

    public Sprite[] Sprites;
    public Sprite[] CustomSprites;

    public string[] paths;

    public Canvas Error;

    private Sprite[] Backgrounds;

    private System.Random rnd = new System.Random();

    int i = 0;

    private void OnEnable()
    {
        if (Constants.BackgroundsChanged)
        {
            LoadSprites();
        }
        else
        {
            NextSprite();
        }
    }

    private void LoadSprites()
    {
        if (Constants.CustomBackgrounds)
        {

            List<Sprite> NewSprites = new List<Sprite>();

            for (int i = 0; i < paths.Length; i++)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(paths[i]);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + paths[i]);
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
            Sprites = Resources.LoadAll<Sprite>(Constants.BackgroundPath);
            Backgrounds = Sprites;
        }

        Constants.BackgroundsChanged = false;


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

        if (width > height)
        {
            Vector2 sizeX = new Vector2(worldScreenWidth / width, worldScreenWidth / width);
            transform.localScale = sizeX;
        }
        else if (height > width)
        {
            Vector2 sizeY = new Vector2(worldScreenHeight / height, worldScreenHeight / height);
            transform.localScale = sizeY;
        }
        // move to left side of screen
        float screenLeft = worldScreenWidth * -0.5f;
        float newWidth = width * (worldScreenHeight / height);
        transform.SetPositionAndRotation(new Vector3((newWidth * 0.5f) + screenLeft, 0, 0), Quaternion.identity);
    }


    /// <summary>
    /// Shuffle the array.
    /// </summary>
    /// <typeparam name="T">Array element type.</typeparam>
    /// <param name="array">Array to shuffle.</param>
    private void Shuffle()
    {
        int n = Backgrounds.Length;
        for (int i = 0; i < n; i++)
        {
            // Use Next on random instance with an argument.
            // ... The argument is an exclusive bound.
            //     So we will not go past the end of the array.
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
        Constants.BackgroundsChanged = true;

        Error.gameObject.SetActive(false);
    }
}
