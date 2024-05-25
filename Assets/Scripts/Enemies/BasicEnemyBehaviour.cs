using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBehaviour : MonoBehaviour
{
    public enum Direction { Left, Right };
    private const float COOLDOWN = 1f; // attack cooldown 1 second
    [SerializeField]
    private GameObject player;
    private GameObject prefab;
    public int Health { get; private set; }
    private int attackDamage;
    private Direction facingDirection;
    
    public void TakeDamage(int damage)
    {
        Health -= damage;
    }
    private void Die()
    {

        Destroy(prefab);
    }
    IEnumerator AttackEverySecond()
    {
        while(true)
        {
            Attack();
            yield return new WaitForSeconds(COOLDOWN);
        }
    }
    private void Attack()
    {
        if (transform.position.x - player.transform.position.x < 0.5)
            player.GetComponent<Health>().TakeDamage(attackDamage, new Vector2(0f,0f));

    }

    // Start is called before the first frame update
    void Start()
    {
        Health = 50;
        prefab = gameObject;
        StartCoroutine(AttackEverySecond());
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 1f * Time.deltaTime);
    }
}
