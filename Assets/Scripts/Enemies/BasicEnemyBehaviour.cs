using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public abstract class BasicEnemyBehaviour : MonoBehaviour
{
    public enum Direction { Left, Right };
    protected float COOLDOWN;
    protected float SPEED;
    protected playerInputs player; //change it from serialize
    protected GameObject attackPoint;
    protected GameObject stepCheck;
    protected GameObject wallCheck;
    protected Vector2 knockBack;
    protected float attackDamage;
    protected Direction facingDirection;
    protected Health health;
    protected Renderer objectRenderer;
    protected Rigidbody2D rb;
    protected LayerMask groundMask;
    private bool bounce = false;
    private bool canMove= false;
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
        rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void Start()
    {
        player = playerInputs.Instance;
        StartCoroutine(AttackOnCooldown());
        objectRenderer = GetComponent<Renderer>();
        stepCheck = transform.GetChild(2).gameObject;
        wallCheck = transform.GetChild(3).gameObject;
        groundMask = LayerMask.GetMask("ground");
        facingDirection = player.transform.position.x < transform.position.x ? Direction.Left : Direction.Right; ;

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (canMove)
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
    }
    private bool IsGrounded()
    {
        // Adjust the ground check implementation as needed
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }
    protected virtual void MoveTowardsTarget(Vector3 targetPosition)
    {

        var StepcircleBound = Physics2D.OverlapCircle(stepCheck.transform.position, 0.001f, groundMask);
        var WallCircleBound = Physics2D.OverlapCircle(wallCheck.transform.position, 0.001f, groundMask);
        if (StepcircleBound != null && WallCircleBound == null && !bounce)
        {
            Vector2 currentPosition = transform.position;
            Vector2 newPosition = new Vector2(currentPosition.x, currentPosition.y + 0.5f);
            transform.position = newPosition;
            bounce = true;
        }
        else
        {
            bounce = false;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, SPEED * Time.deltaTime);

        // Check if the target position is to the left and the current facing direction is right
        if (targetPosition.x < transform.position.x && facingDirection == Direction.Right)
        {
            // Flip the direction to left
            facingDirection = Direction.Left;
            Flip(facingDirection);
        }
        // Check if the target position is to the right and the current facing direction is left
        else if (targetPosition.x > transform.position.x && facingDirection == Direction.Left)
        {
            // Flip the direction to right
            facingDirection = Direction.Right;
            Flip(facingDirection);
        }
    }
    void Flip(Direction direction)
    {
        // Flip the localRotation.y by setting the correct Euler angles
        if (direction == Direction.Right)
            transform.localRotation = Quaternion.Euler(0, 180f, 0);
        else
            transform.localRotation = Quaternion.Euler(0, 0, 0);
    }


    public void EnableMovement()
    {
            canMove = true;
    }
}
