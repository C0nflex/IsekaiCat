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
    public AudioClip meleeAttackSound;
    public AudioClip rangeAttackSound;
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
        rangedAttackParticles.gameObject.SetActive(false);
        _rigidBody.AddForce(Vector2.up * jumpspeed, ForceMode2D.Impulse);
        base.Jump();
        rangedAttackParticles.gameObject.SetActive(true);
    }

    protected override void BasicAttack()
    {
        audioSource.PlayOneShot(meleeAttackSound);
        List<Collider2D> enemiesHit = new List<Collider2D>();
        foreach (LayerMask EnemyLayer in EnemyLayers)
        {
            enemiesHit.AddRange(Physics2D.OverlapCircleAll(transform.position, attackRadius, EnemyLayer));
        }
        foreach (Collider2D enemy in enemiesHit)
        {
            if (enemy.tag == "Enemy")
                enemy.GetComponent<Health>().TakeDamage(basicAttackDamage, basicAttackKnockback, gameObject);

            if (enemy.name == "Ogre" || enemy.name == "Ogre (1)" || enemy.name == "Ogre (2)" || enemy.name == "Ogre (3)" || enemy.name == "Ogre (4)" || enemy.name == "Ogre (5)" || enemy.name == "Ogre (6)" || enemy.name == "Ogre (7)" || enemy.name == "Ogre (8)" || enemy.name == "Ogre (9)" || enemy.name == "Ogre (10)" || enemy.name == "Ogre (11)" || enemy.name == "Ogre (12)" || enemy.name == "Ogre (13)" || enemy.name == "Ogre (14)" || enemy.name == "Ogre (15)" || enemy.name == "Ogre (16)" || enemy.name == "Ogre (17)" || enemy.name == "Ogre (18)" || enemy.name == "Ogre (19)")
            {
                Health health = enemy.GetComponent<Health>();
                if (!health._Dead)
                    audioSource.PlayOneShot(OgreHit);
                else
                    audioSource.PlayOneShot(OgreDie);
            }
            else if (enemy.name == "Bat" || enemy.name == "Bat (1)" || enemy.name == "Bat (2)" || enemy.name == "Bat (3)" || enemy.name == "Bat (4)" || enemy.name == "Bat (5)" || enemy.name == "Bat (6)" || enemy.name == "Bat (7)" || enemy.name == "Bat (8)" || enemy.name == "Bat (9)" || enemy.name == "Bat (10)" || enemy.name == "Bat (11)" || enemy.name == "Bat (12)" || enemy.name == "Bat (13)" || enemy.name == "Bat (14)" || enemy.name == "Bat (15)" || enemy.name == "Bat (16)" || enemy.name == "Bat (17)" || enemy.name == "Bat (18)" || enemy.name == "Bat (19)")
            {
                Health health = enemy.GetComponent<Health>();
                if (!health._Dead)
                    audioSource.PlayOneShot(BatHit);
                else
                    audioSource.PlayOneShot(BatDie);
            }
        }
        base.BasicAttack();
    }

    protected override void RangedAttack()
    {
        // Calculate the direction from the spawn point to the mouse position

        // Instantiate and initialize the projectile
        audioSource.PlayOneShot(rangeAttackSound);
        var ProjectileSpawned = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        var direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        direction = new Vector3(direction.x, direction.y, transform.position.z).normalized;
        ProjectileSpawned.GetComponent<ProjectileManager>().Init(direction,projectileSpeed,projectileDamage, projectileKnockback, projectileGravity);
        base.RangedAttack();
    }


}
