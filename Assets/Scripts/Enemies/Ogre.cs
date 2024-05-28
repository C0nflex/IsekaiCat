using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Ogre : BasicEnemyBehaviour
{
    private GameObject batPrefab;
    private Animator animator;
    [SerializeField] private AudioClip hogerHit;
    private AudioSource audioSource;
    protected override void Attack()
    {
        if (player != null)
        {
            audioSource.Play();
            if (math.abs(attackPoint.transform.position.x - player.transform.position.x) < 0.8 &&
            math.abs(attackPoint.transform.position.y - player.transform.position.y) < 0.8)
                player.GetComponent<Health>().TakeDamage(attackDamage, knockBack, gameObject);
        }
        
    }
    private void PerformAttack()
    {
        Attack();
    }
    protected override IEnumerator AttackOnCooldown()
    {
        while (true)
        {
            yield return new WaitForSeconds(COOLDOWN);
            animator.SetTrigger("OgreAttack");
        }
    }
    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        //ogre stats
        COOLDOWN = 1.5f;
        SPEED = 0.7f;
        attackDamage = 30f;
        health._startingHealth = 150f;
        animator = GetComponent<Animator>();
        stepCheck = transform.GetChild(2).gameObject;
        wallCheck = transform.GetChild(3).gameObject;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = hogerHit;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
