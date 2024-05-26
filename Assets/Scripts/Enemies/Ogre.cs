using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Ogre : BasicEnemyBehaviour
{
    private GameObject batPrefab;
    private Animator animator;
    protected override void Attack()
    {
        if (math.abs(attackPoint.transform.position.x - player.transform.position.x) < 0.8 &&
            math.abs(attackPoint.transform.position.y - player.transform.position.y) < 0.8)
            player.GetComponent<Health>().TakeDamage(attackDamage, knockBack, gameObject);
        animator.SetTrigger("OgreAttack");
    }
    protected override IEnumerator AttackOnCooldown()
    {
        System.Random rand = new System.Random();
        int choice = rand.Next() % 1;
        while (true)
        {
            if (choice == 0)
            {
                Attack();
                COOLDOWN = 1.5f;
            }
            else
            {
                throwBat();
                COOLDOWN = 3f;
            }
                yield return new WaitForSeconds(COOLDOWN);
        }
    }
    private void throwBat()
    {

    }

    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        //ogre stats
        COOLDOWN = 1f;
        SPEED = 0.7f;
        attackDamage = 30f;
        health._startingHealth = 150f;
        animator = GetComponent<Animator>();
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

    }
}
