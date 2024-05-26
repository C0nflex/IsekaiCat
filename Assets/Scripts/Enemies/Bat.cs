using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Bat : BasicEnemyBehaviour
{
    private GameObject batPrefab;
    private Collider2D coll;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Attack();
            StartCoroutine(resetVelocity());
            //health.TakeDamage(0, player.basicAttackKnockback, player.gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(resetVelocity());
            //health.TakeDamage(0, player.basicAttackKnockback, player.gameObject);
        }
    }
    protected override void Attack()
    {
        //if (math.abs(attackPoint.transform.position.x - player.transform.position.x) < 0.8 &&
            //math.abs(attackPoint.transform.position.y - player.transform.position.y) < 0.8)
            player.GetComponent<Health>().TakeDamage(attackDamage, knockBack, gameObject);
    }
    protected override IEnumerator AttackOnCooldown()
    {
        yield return new WaitForSeconds(COOLDOWN);
    }

    protected override void Awake()
    {
        base.Awake();
        COOLDOWN = 0f;
        SPEED = 1.7f;
        attackDamage = 10f;
        health._startingHealth = 50f;
    }
    private void resetVelocity(object sender, EventArgs e)
    {
        StartCoroutine(resetVelocity());
    }
    private IEnumerator resetVelocity()
    {
        yield return new WaitForSeconds(0.75f);
        rb.velocity = new Vector2(0, 0);
        rb.angularVelocity = 0;
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        //ogre stats
        health.onHurt += resetVelocity;
        base.Start();
        
        
    }
    protected override void Flip()
    {
        // Flip the localRotation.y by setting the correct Euler angles
        if (facingDirection == Direction.Right)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        else
            transform.localRotation = Quaternion.Euler(0, 180f, 0);
    }

    protected override void MoveTowardsTarget()
    {
        Vector3 targetPosition = player.transform.position;
        //Vector2 direction = (targetPosition - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, SPEED * Time.deltaTime);
        Debug.Log(rb.velocity);
        //rb.velocity = direction * SPEED;
        Flip(targetPosition);
    }
    // Update is called once per frame
    protected override void Update()
    {

        base.Update();

    }
}
