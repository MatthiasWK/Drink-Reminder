using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScaler : MonoBehaviour {

	void Start () {
        Utilities.ResizeSpriteToScreen(gameObject, Camera.main, 1);
	}

}
