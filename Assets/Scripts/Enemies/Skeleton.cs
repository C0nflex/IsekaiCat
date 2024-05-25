using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Skeleton : BasicEnemyBehaviour
{
    protected override IEnumerator AttackOnCooldown()
    {
        while (true)
        {
            Attack();
            yield return new WaitForSeconds(COOLDOWN);
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //skeleton stats
        COOLDOWN = 1f;
        SPEED = 1f;
        attackDamage = 20f;
        health._startingHealth = 100f;

        healthText.text = health._currentHealth.ToString();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Attack()
    {
        if (math.abs(attackPoint.transform.position.x - player.transform.position.x) < 0.8 &&
           math.abs(attackPoint.transform.position.y - player.transform.position.y) < 0.8)
            player.GetComponent<Health>().TakeDamage(attackDamage, knockBack, gameObject);
    }
}
