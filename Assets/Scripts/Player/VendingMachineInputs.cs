using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, groundSlamRadius, EnemyLayer);
        foreach (Collider2D enemy in enemiesHit)
            enemy.GetComponent<Health>().TakeDamage(basicAttackDamage, basicAttackKnockback, gameObject);
        base.BasicAttack();
        Debug.Log("slam");
        isMidSlam = false;
        didstartSlam = false;
        isMidJump = false;
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
        base.RangedAttack();
    }

}
