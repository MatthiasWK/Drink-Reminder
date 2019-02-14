using UnityEngine;

public class SelectionShapeController : MonoBehaviour {

    private bool isDragging = false;
    private Vector3 offset;
    public GameObject image;
    public ImageEditor EditorScript;

    void Update ()
    {
        // User can move the shape by touching and then dragging it. 'CustomCutout()' is called every frame that the object is dragged, so the image is updated automatically.
        if (Input.GetMouseButton(0))
        {
            var inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (!isDragging)
            {
                RaycastHit2D hit = Physics2D.Raycast(inputPosition, inputPosition, 0.5f);
                if (hit.collider != null && hit.transform == transform)
                {
                    isDragging = true;
                    offset = transform.position - inputPosition;
                }
                
            }
            else
            {
                transform.position = inputPosition + offset;
                CheckBounds();
                EditorScript.CustomCutout();
            }
        }
        else
        {
            if (isDragging)
            {
                isDragging = false;
            }
        }
	}
    /// <summary>
    /// makes sure that the selection shape stays within the bounds of the picture
    /// </summary>
    private void CheckBounds()
    {
        var shapeSize = GetComponent<SpriteRenderer>().bounds.size;
        var imageSize = image.GetComponent<SpriteRenderer>().bounds.size;

        if (transform.position.x < image.transform.position.x)
        {
            transform.position = new Vector2(image.transform.position.x, transform.position.y);
        }
        else if (transform.position.x + shapeSize.x > image.transform.position.x + imageSize.x)
        {
            transform.position = new Vector2(image.transform.position.x + imageSize.x - shapeSize.x, transform.position.y);
        }

        if (transform.position.y < image.transform.position.y)
        {
            transform.position = new Vector2(transform.position.x, image.transform.position.y);
        }
        else if (transform.position.y + shapeSize.y > image.transform.position.y + imageSize.y)
        {
            transform.position = new Vector2(transform.position.x, image.transform.position.y + imageSize.y - shapeSize.y);
        }
    }

    public void ScaleSize(float value)
    {
        transform.localScale = new Vector3(value, value, value);
        CheckBounds();
        EditorScript.CustomCutout();
    }
}
