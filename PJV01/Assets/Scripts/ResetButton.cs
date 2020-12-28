using UnityEngine;
using System.Collections;
public class ResetButton : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    public Color highlightColor = Color.white;

    public void OnMouseOver()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.color = highlightColor;
        }
    }
    public void OnMouseExit()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.color = Color.white;
        }
    }
    public void OnMouseDown()
    {
        transform.localScale *= 1.1f;
    }
    public void OnMouseUp()
    {
        transform.localScale /= 1.1f;
        sceneController.Reset();
    }
}