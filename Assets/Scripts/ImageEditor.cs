using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImageEditor : MonoBehaviour {

    public SpriteRenderer SourceImage;
    public SpriteRenderer SourceBackground;
    public SpriteRenderer Cutout;
    public SpriteRenderer[] TargetSprites;
    public string[] CutoutSprites;
    public Color[] Colors;
    private int i = 0;

    public Text DebugText;

    private string ID;

    private void Start()
    {
        ID = GameController.tmp_Name;

        LoadPrevious();
        ResizeSourceSprite();

        var readableText = DuplicateTexture(SourceImage.sprite.texture);

        Sprite mySprite = Sprite.Create(readableText, new Rect(0.0f, 0.0f, readableText.width, readableText.height), new Vector2(0f, 0f), 100.0f);

        SourceImage.sprite = mySprite;
        ResizeSourceSprite();
    }

    private void OnEnable()
    {
        Cutout.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        Cutout.gameObject.SetActive(false);
    }
    /// <summary>
    /// Saves a texture as png
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="tex"></param>
    private void SaveFile(string fileName, Texture2D tex)
    {
        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();

        File.WriteAllBytes(Application.persistentDataPath + "/" + fileName + ".png", bytes);
    }
     public void SaveAll()
    {
        for(int s = 0; s < TargetSprites.Length; s++)
        {
            var tex = DuplicateTexture(TargetSprites[s].sprite.texture);
            SaveFile(ID + "_shape_" + s, tex);
        }

        Constants.ShapesChanged = true;
    }

    /// <summary>
    /// Loads all previously saved sprites into the game
    /// </summary>
    public void LoadPrevious()
    {
        for (int t = 0; t < TargetSprites.Length; t++)
        {
            Texture2D texture = NativeGallery.LoadImageAtPath(Application.persistentDataPath + "/" + ID + "_shape_" + t + ".png");
            if (texture == null)
            {
                print("Couldn't load texture from " + Application.persistentDataPath + "/" + ID + "_shape_" + t + ".png");
                TargetSprites[t].sprite = Resources.Load<Sprite>("DefaultShapes/shape_" + t);
            }
            else
            {
                Sprite mySprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                TargetSprites[t].sprite = mySprite;
            }
        }
        
    }

    /// <summary>
    /// Cuts an image out of the source image in the shape of the current selection image, pastes it to the current target image
    /// </summary>
    public void CustomCutout()
    {

        int w = Cutout.sprite.texture.width;
        int h = Cutout.sprite.texture.height;

        Texture2D b = new Texture2D(w, h);
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Color c = Cutout.sprite.texture.GetPixel(x, y);

                if (c.a == 0)
                {
                    b.SetPixel(x, y, Color.clear);
                }
                else
                {
                    Vector3 cW = Cutout.transform.position + (new Vector3(x, y) / Cutout.sprite.pixelsPerUnit * Cutout.transform.localScale.x);
                    var LocalPos = cW - SourceImage.transform.position;
                    var uvX = LocalPos.x / SourceImage.bounds.size.x * SourceImage.sprite.texture.width;
                    var uvY = LocalPos.y / SourceImage.bounds.size.y * SourceImage.sprite.texture.height;

                    Color c2 = SourceImage.sprite.texture.GetPixel(Mathf.RoundToInt(uvX), Mathf.RoundToInt(uvY));
                    b.SetPixel(x, y, c2);
                }
            }
        }
        b.Apply();

        Sprite mySprite = Sprite.Create(b, new Rect(0.0f, 0.0f, b.width, b.height), new Vector2(0.5f, 0.5f), 100.0f);

        TargetSprites[i].sprite = mySprite;
    }

    /// <summary>
    /// Change the current target and cutout
    /// </summary>
    /// <param name="s"></param>
    public void ChangeSelected(int s)
    {
        i = s;
        Cutout.sprite = Resources.Load<Sprite>(CutoutSprites[i]);
        Cutout.GetComponent<SpriteOutline>().color = Colors[i];
    }

    /// <summary>
    /// Gets an Image from the Gallery using the NativeGallery Plugin
    /// </summary>
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
                
                var newTexture = DuplicateTexture(texture);
                Sprite mySprite = Sprite.Create(newTexture, new Rect(0.0f, 0.0f, newTexture.width, newTexture.height), new Vector2(0f, 0f), 100.0f);
                
                SourceImage.sprite = mySprite;
            }
        }, "Select a PNG image", "image/png");

        Debug.Log("Permission result: " + permission);
        ResizeSourceSprite();
    }

    /// <summary>
    /// Textures loaded from the gallery aren't initially readable, so this method duplicates the texture onto a new readable one
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    Texture2D DuplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    /// <summary>
    /// Resizes a the source sprite to fit within the view square
    /// </summary>
    /// <param name="theSprite"></param>
    /// <param name="theCamera"></param>
    /// <param name="fitToScreenWidthHeight"></param>
    private void ResizeSourceSprite()
    {

        transform.localScale = new Vector3(1, 1, 1);

        float width = SourceImage.sprite.bounds.size.x;
        float height = SourceImage.sprite.bounds.size.y;

        float backgroundWidth = SourceBackground.sprite.bounds.size.x;
        float backgroundHeight = SourceBackground.sprite.bounds.size.y;

        if (width > height)
        {
            Vector2 sizeX = new Vector2(backgroundWidth / width, backgroundWidth / width);
            SourceImage.transform.localScale = sizeX;

            SourceImage.transform.localPosition = new Vector3(-0.5f, height * -0.5f * sizeX.y);
        }
        else if (height > width)
        {
            Vector2 sizeY = new Vector2(backgroundHeight / height, backgroundHeight / height);
            SourceImage.transform.localScale = sizeY;

            SourceImage.transform.localPosition = new Vector3(width * -0.5f * sizeY.x, -0.5f);

        }
    }
}
