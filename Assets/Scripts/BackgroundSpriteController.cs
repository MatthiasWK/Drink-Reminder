using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpriteController : MonoBehaviour {

    public Sprite[] Sprites;
    public Sprite[] CustomSprites;

    private Sprite[] Backgrounds;

    int i = 0;

	void Start ()
    {
        
        if (Constants.Custom)
        {
            Backgrounds = CustomSprites;
        }
        else
        {
            Sprites = Resources.LoadAll<Sprite>(Constants.Path);
            Backgrounds = Sprites;
        }
        
        SetSprite(0);
    }

    private void OnEnable()
    {
        Start();
    }

    public void NextSprite()
    {
        i = (i +1) % Backgrounds.Length;
        SetSprite(i);
    }

    private void SetSprite(int s)
    {
        GetComponent<SpriteRenderer>().sprite = Backgrounds[s];

        Utilities.ResizeSpriteToScreen(gameObject, Camera.main, 1);
    }

    public void SetCustomSprites(Sprite[] NewSprites)
    {
        CustomSprites = NewSprites;
    }
}
