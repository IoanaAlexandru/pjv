using System.Collections;
using UnityEngine;

public class ZombieController : PhysicsObject
{
    public GameObject player;

    public float maxSpeed = 6;
    public float leftLimit, rightLimit;
    public float attackInterval = 1f;

    public GameObject projectile;

    CapsuleCollider2D zombieCollider;

    public bool movingLeft = true;
    bool dead = false;
    bool waiting = false;
    bool attacking = false;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Use this for initialization
    void Awake()
    {
        zombieCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        StartCoroutine(Attack());
    }

    protected override void ComputeVelocity()
    {
        if (dead || waiting || attacking)
        {
            targetVelocity = new Vector2(0, 0);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            return;
        }

        var pos = gameObject.transform.position.x;
        if (movingLeft && pos <= leftLimit)
        {
            StartCoroutine(WaitAndTurn());
        }
        else if (!movingLeft && pos >= rightLimit)
        {
            StartCoroutine(WaitAndTurn());
        }

        Vector2 move = new Vector2(movingLeft ? -1 : 1, 0);

        if (move.x > 0.01f)
        {
            if (spriteRenderer.flipX == true)
            {
                spriteRenderer.flipX = false;
            }
        }
        else if (move.x < -0.01f)
        {
            if (spriteRenderer.flipX == false)
            {
                spriteRenderer.flipX = true;
            }
        }

        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }

    IEnumerator WaitAndTurn()
    {
        waiting = true;
        yield return new WaitForSeconds(1f);
        waiting = false;
        movingLeft = !movingLeft;
    }

    IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);

            if (Vector3.Distance(gameObject.transform.position, player.transform.position) > 10)
            {
                continue;
            }

            if (dead)
            {
                break;
            }

            attacking = true;

            // Trigger attack animation
            animator.Play("Zombie_Attack");
            var animationDuration = animator.GetCurrentAnimatorClipInfo(0).Length;

            // Instantiate projectile
            yield return new WaitForSeconds(animationDuration / 2);
            ((ProjectileBehaviour)projectile.GetComponent<MonoBehaviour>()).movingLeft = movingLeft;
            Instantiate(projectile, gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(animationDuration / 2);

            attacking = false;
        }
    }

    void OnTriggerEnter2D(Collider2D theCollider)
    {
        if (theCollider.CompareTag("Player"))
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        dead = true;
        animator.Play("Zombie_Dead");
        SceneController.Instance.score++;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length + 0.5f);
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            spriteRenderer.color = new Color(1, 1, 1, i);
            yield return null;
        }
        Destroy(gameObject);
    }
}