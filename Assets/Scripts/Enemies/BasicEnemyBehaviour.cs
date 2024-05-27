using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Cinemachine;

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
    private bool firstTimeOnCamera = true;
    private bool isAttacking = false;
    virtual protected void Die()
    {
        Destroy(gameObject);
        playerInputs.Instance.increaseSoulLevel(1);
    }
    virtual protected IEnumerator AttackOnCooldown()
    {
        while (true)
        {
            if(canMove)
                Attack();
            yield return new WaitForSeconds(COOLDOWN);
        }
    }
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
        //virtualCamera = GameObject.Find("Virtual Camera");
        player = playerInputs.Instance;
        objectRenderer = GetComponent<Renderer>();
        groundMask = LayerMask.GetMask("ground");
        if(player.transform.position.x < transform.position.x)
            facingDirection = Direction.Right;
        else
            facingDirection = Direction.Left;

        Flip();
        //StartCoroutine(AttackOnCooldown());
    }

    // Update is called once per frame
    private bool IsVisibleByCamera(Camera camera)
    {
        if (camera == null)
        {
            return false;
        }

        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(frustumPlanes, objectRenderer.bounds);
    }
    protected virtual void Update()
    {
        if(objectRenderer.isVisible && !firstTimeOnCamera)
            canMove = true;
        else
            canMove = false;

        if (canMove)
        {
            MoveTowardsTarget();
        }
        if(canMove && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackOnCooldown());
        }
    }
    protected virtual void MoveTowardsTarget()
    {
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        var StepcircleBound = Physics2D.OverlapCircle(stepCheck.transform.position, 0.01f, groundMask);
        var WallCircleBound = Physics2D.OverlapCircle(wallCheck.transform.position, 0.01f, groundMask);
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
        Flip(targetPosition);
    }
    protected void Flip(Vector3 targetPosition)
    {
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
    protected virtual void Flip()
    {
        // Flip the localRotation.y by setting the correct Euler angles
        if (facingDirection == Direction.Right)
            transform.localRotation = Quaternion.Euler(0, 180f, 0);
        else
            transform.localRotation = Quaternion.Euler(0, 0, 0);
    }


    public void EnableMovement()
    {
        firstTimeOnCamera = false;
    }
}
