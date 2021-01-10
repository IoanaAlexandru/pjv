using System.Collections;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float speed = 3;
    public bool movingLeft = true;

    bool hitSomething = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!movingLeft)
        {
            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!hitSomething)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime * (movingLeft ? -1 : 1), Space.World);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Through"))
        {
            StartCoroutine(Disappear());
        }
    }

    IEnumerator Disappear()
    {
        hitSomething = true;
        var collider = gameObject.GetComponent<CapsuleCollider2D>();
        collider.enabled = false;
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            transform.localScale = transform.localScale * i;
            yield return null;
        }
        Destroy(gameObject);
    }
}
