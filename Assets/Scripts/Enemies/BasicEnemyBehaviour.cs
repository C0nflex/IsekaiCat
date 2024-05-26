using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using TMPro;

public abstract class BasicEnemyBehaviour : MonoBehaviour
{
    public enum Direction { Left, Right };
    protected float COOLDOWN;
    protected float SPEED;
    protected playerInputs player; //change it from serialize
    protected GameObject attackPoint;
    protected Vector2 knockBack;
    protected float attackDamage;
    protected Direction facingDirection;
    protected Health health;
    protected Renderer objectRenderer;
    protected Rigidbody2D rb;

    virtual protected void Die()
    {
        Destroy(gameObject);
    }
    abstract protected IEnumerator AttackOnCooldown();
    abstract protected void Attack();

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        health = gameObject.GetComponent<Health>();
        attackPoint = transform.Find("AttackPointEnemy").gameObject;
    }
    protected virtual void Start()
    {
        player = playerInputs.Instance;
        StartCoroutine(AttackOnCooldown());
        objectRenderer = GetComponent<Renderer>();

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (objectRenderer.isVisible)
        {
            Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

            if (player != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, SPEED * Time.deltaTime);
                MoveTowardsTarget(targetPosition);
            }
        }
    }
    protected virtual void MoveTowardsTarget(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, SPEED * Time.deltaTime);

        // Check if the target position is to the left and the current facing direction is right
        if (targetPosition.x < transform.position.x && facingDirection == Direction.Right)
        {
            // Flip the direction to left
            facingDirection = Direction.Left;
            Flip();
        }
        // Check if the target position is to the right and the current facing direction is left
        else if (targetPosition.x > transform.position.x && facingDirection == Direction.Left)
        {
            // Flip the direction to right
            facingDirection = Direction.Right;
            Flip();
        }
    }
    void Flip()
    {
        // Flip the localScale.x by multiplying with -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
