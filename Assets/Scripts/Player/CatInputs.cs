using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatInputs : playerInputs
{
    [SerializeField] private float jumpspeed;
    [SerializeField] private float jumpCutMultiplier;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDamage;
    [SerializeField] private Vector2 projectileKnockback;
    [SerializeField] private float projectileGravity;
    protected override void HandleJump()
    {
        if (!jumpOnCooldown && ((Input.GetKeyDown(KeyCode.Space) && lastGroundedTime > 0) && !isMidJump || (Input.GetKeyDown(KeyCode.Space) & isGoingUpStairs)))
        {
            Jump();
        }
        if (Input.GetKeyUp(KeyCode.Space) && _rigidBody.velocity.y > 0)
        {
            //_rigidBody.AddForce(Vector2.down * _rigidBody.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }
    }

    protected override void Jump()
    {
        _rigidBody.AddForce(Vector2.up * jumpspeed, ForceMode2D.Impulse);
        base.Jump();
    }

    protected override void BasicAttack()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, EnemyLayer);
        foreach (Collider2D enemy in enemiesHit)
        {
            enemy.GetComponent<Health>().TakeDamage(basicAttackDamage, basicAttackKnockback, gameObject);
        }
        base.BasicAttack();
    }

    protected override void RangedAttack()
    {
        var ProjectileSpawned = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        print(Input.mousePosition);
        var direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        direction = new Vector3(direction.x, direction.y, transform.position.z).normalized;
        ProjectileSpawned.GetComponent<ProjectileManager>().Init(direction,projectileSpeed,projectileDamage, projectileKnockback, projectileGravity);
        base.RangedAttack();
    }

    
}
