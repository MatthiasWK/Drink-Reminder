using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSizeChanger : MonoBehaviour {

    public Text Size;

    private void Start()
    {
        Size.text = Constants.Rows + " x " + Constants.Columns;
    }

    public void ChangeGridSize(int Change)
    {
        
        if (Change == 0)
        {
            Constants.Rows -= 1;
            Constants.Columns -= 1;
        }
        else if (Change == 1)
        {
            Constants.Rows += 1;
            Constants.Columns += 1;
        }

        Size.text = Constants.Rows + " x " + Constants.Columns;
    }
}
