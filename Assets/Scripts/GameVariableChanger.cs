using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameVariableChanger : MonoBehaviour {

    public Text Size;
    public Text Num;
    public Text Name;

    public GameObject[] ShapePrefabs;
    //public GameObject[] BonusPrefabs;

    private void Start()
    {
        Size.text = Constants.Rows + " x " + Constants.Columns;
        Num.text = Constants.NumShapes.ToString();
        SetTheme();
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
        Constants.Theme = (Constants.Theme + Change) % 2;

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
    }
}
