using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class VendingMachineInputs : playerInputs
{
    [SerializeField] private Transform projectileSpawnPoint;
    [Header("ShockwaveGrenade")]
    [SerializeField] private GameObject shockwaveGrenadePrefab;
    [SerializeField] private float shockwaveGrenadeSpeed;
    [SerializeField] private Vector2 shockwaveGrenadeKnockBack;
    [SerializeField] private float shockwaveGrenadeGravity;
    [Header("GroundSlam")]
    [SerializeField] private float groundSlamLaunchSpeed;
    [SerializeField] private float groundSlamDamage;
    [SerializeField] private float groundSlamRadius;
    [SerializeField] private float groundSlamFallAndHangGravityMultiplayer;
    [SerializeField] private float TimeUntilDrop;
    [SerializeField] private Collider2D topCollider;
    public bool isMidSlam = false;
    public bool didstartSlam = false;
    [SerializeField] private Vector2 groundSlamKnockBack;
    [Header("DamageGrenade")]
    [SerializeField] private GameObject damageGrenadePrefab;
    [SerializeField] private float damageGrenadeSpeed;
    [SerializeField] private float damageGrenadeDamage;
    [SerializeField] private Vector2 damageGrenadeKnockBack;
    [SerializeField] private float damageGrenadeGravity;

    public AudioClip slam;
    public AudioClip grenade;
    public AudioClip shockwave;

    protected override void HandleJump()
    {
        if (!jumpOnCooldown && !isMidJump && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("green");
        }
    }

    protected override void Jump()
    {
        var ProjectileSpawned = Instantiate(shockwaveGrenadePrefab, projectileSpawnPoint.position, Quaternion.identity);
        var direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        direction = new Vector3(direction.x, direction.y, transform.position.z).normalized;
        ProjectileSpawned.GetComponent<ProjectileManager>().Init(direction,shockwaveGrenadeSpeed, 0, shockwaveGrenadeKnockBack, shockwaveGrenadeGravity);
        audioSource.PlayOneShot(shockwave);
        base.Jump();
    }

    protected override void BasicAttack()
    {
        if (!isMidJump)
        {
            _rigidBody.velocity = new Vector3(0, groundSlamLaunchSpeed, 0);
            StartCoroutine(WaitToDropSlam());
            //_rigidBody.AddForce(Vector2.up * groundSlamLaunchSpeed, ForceMode2D.Impulse);
            didstartSlam = true;
            isMidJump = true;
            
        }
    }

    IEnumerator WaitToDropSlam()
    {
        yield return new WaitForSeconds(TimeUntilDrop);
        _rigidBody.velocity = new Vector3(0, -groundSlamLaunchSpeed, 0);
        _rigidBody.gravityScale = gravityScale * groundSlamFallAndHangGravityMultiplayer;
        isMidSlam = true;
    }



    protected override void Update()
    {
        if (didstartSlam && !isMidSlam && Physics2D.OverlapAreaAll(topCollider.bounds.min, topCollider.bounds.max, groundMasks[0]).Length > 0)
            _rigidBody.gravityScale = 0;
        base.Update();
    }



    protected override void Grounded()
    {
        base.Grounded();
        if (isMidSlam)
            Slam();
    }

    private void Slam()
    {
        anim.SetTrigger("slam");
        List<Collider2D> enemiesHit = new List<Collider2D>();
        foreach (LayerMask EnemyLayer in EnemyLayers)
        {
            enemiesHit.AddRange(Physics2D.OverlapCircleAll(transform.position, groundSlamRadius, EnemyLayer));
        }
        foreach (Collider2D enemy in enemiesHit)
        {
            if(enemy.tag == "Enemy")
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
        Debug.Log("slam");
        isMidSlam = false;
        didstartSlam = false;
        isMidJump = false;
        audioSource.PlayOneShot(slam);
    }

    protected override void InHangTime()
    {
        if (didstartSlam)
        {

        }
        else
            base.InHangTime();
    }

    protected override void InFallTime()
    {
        if (didstartSlam)
        {

        }
        else
            base.InFallTime();
    }

    protected override void RangedAttack()
    {
        anim.SetTrigger("red");
    }

    private void SpawnGranede()
    {
        var ProjectileSpawned = Instantiate(damageGrenadePrefab, projectileSpawnPoint.position, Quaternion.identity);
        var direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        direction = new Vector3(direction.x, direction.y, transform.position.z).normalized;
        ProjectileSpawned.GetComponent<ProjectileManager>().Init(direction,
        damageGrenadeSpeed, damageGrenadeDamage, damageGrenadeKnockBack, damageGrenadeGravity);
        audioSource.PlayOneShot(grenade);
        base.RangedAttack();
    }

}
