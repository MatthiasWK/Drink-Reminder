using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageEditor : MonoBehaviour {

    public SpriteRenderer SourceImage;
    public SpriteRenderer Cutout;
    public SpriteRenderer TargetImage;

    //public void set(Texture2D tex)
    //{
    //    sourceMesh.sharedMaterial.mainTexture = tex;
    //}

    //public void saveFile(string fileName, Texture2D tex)
    //{
    //    // Encode texture into PNG
    //    byte[] bytes = tex.EncodeToPNG();

    //    File.WriteAllBytes(Application.dataPath + "/" + fileName + ".png", bytes);
    //}

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

                if (c == Color.black)
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

        TargetImage.sprite = mySprite;
    }
}
