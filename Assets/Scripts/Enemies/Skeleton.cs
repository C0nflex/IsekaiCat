using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Skeleton : BasicEnemyBehaviour
{
    private GameObject arrow;
    private Transform projectileSpawnPoint;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private Vector2 arrowKnockback;
    [SerializeField] private float arrowGravity;

    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        //skeleton stats
        COOLDOWN = 2f;
        SPEED = 1f;
        attackDamage = 15f;
        arrowSpeed = 1.75f;
        arrowKnockback = new Vector2(1f, 0);
        arrowGravity = 0;
        health._startingHealth = 100f;
        stepCheck = transform.GetChild(2).gameObject;
        wallCheck = transform.GetChild(3).gameObject;
        projectileSpawnPoint = transform.GetChild(4).transform;
        arrow = GameManager.Instance.arrow;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    protected override void Flip()
    {
        // Flip the localRotation.y by setting the correct Euler angles
        if (facingDirection == Direction.Right)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        else
            transform.localRotation = Quaternion.Euler(0, 180f, 0);
    }
    
    protected override void Attack()
    {
        if (player != null)
        {
            var ProjectileSpawned = Instantiate(arrow, projectileSpawnPoint.position, Quaternion.identity);
            Vector3 targetPos = player.transform.position;
            Vector3 direction = new Vector3(targetPos.x, 0, 0) * ((facingDirection == Direction.Left) ? -1 : 1);
            ProjectileSpawned.GetComponent<ProjectileManager>().Init(direction, arrowSpeed, attackDamage, arrowKnockback, arrowGravity, facingDirection == Direction.Right ? false : true);
        }
    }
}
