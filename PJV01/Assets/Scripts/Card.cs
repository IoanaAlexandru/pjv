using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;

    public int id;
    private SpriteRenderer spriteRenderer;
    public Sprite cardFront;
    private Sprite cardBack;

    public Card(int id, Sprite cardFront)
    {
        this.id = id;
        this.cardFront = cardFront;
    }

    public void SetCard(int id, Sprite cardFront)
    {
        this.id = id;
        this.cardFront = cardFront;
    }

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        cardBack = spriteRenderer.sprite;
    }

    void Reveal()
    {
        spriteRenderer.sprite = cardFront;
        sceneController.OnReveal(this);
    }

    public void Hide()
    {
        spriteRenderer.sprite = cardBack;
    }

    void OnMouseDown()
    {
        if (sceneController.canReveal)
        {
            Reveal();
        } else
        {
            Debug.Log("Can't reveal another card yet.");
        }
    }
}
