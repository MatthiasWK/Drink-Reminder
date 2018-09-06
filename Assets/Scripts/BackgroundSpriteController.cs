using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpriteController : MonoBehaviour {

    public Sprite[] Sprites;
    int i = 0;

	void Start () {
        Sprites = Resources.LoadAll<Sprite>(Constants.Path);
        SetSprite(0);
	}

    public void NextSprite()
    {
        i = (i +1) % Sprites.Length;
        SetSprite(i);
    }

    private void SetSprite(int s)
    {
        GetComponent<SpriteRenderer>().sprite = Sprites[s];

        Utilities.ResizeSpriteToScreen(gameObject, Camera.main, 1);
    }
}
