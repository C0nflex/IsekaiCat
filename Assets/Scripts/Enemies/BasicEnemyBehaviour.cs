using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using TMPro;

public abstract class BasicEnemyBehaviour : MonoBehaviour
{
    public enum Direction { Left, Right };
    protected float COOLDOWN;
    protected float SPEED;
    protected GameObject player; //change it from serialize
    protected GameObject attackPoint;
    protected TextMeshProUGUI healthText;
    protected Vector2 knockBack;
    protected float attackDamage;
    protected Direction facingDirection;
    protected Health health;
    
    virtual protected void Die()
    {
        Destroy(gameObject);
    }
    abstract protected IEnumerator AttackOnCooldown();
    abstract protected void Attack();

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        health = gameObject.GetComponent<Health>();
        attackPoint = transform.Find("AttackPointEnemy").gameObject;
        healthText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    protected virtual void Start()
    {
        player = playerInputs.Instance.gameObject;
        StartCoroutine(AttackOnCooldown());

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, SPEED * Time.deltaTime);
        healthText.text = health._currentHealth.ToString(); //gotta change that with TakeDamage
    }
}
