using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBehaviour : MonoBehaviour
{
    public enum Direction { Left, Right };
    private const int COOLDOWN = 1; // attack cooldown 1 second
    [SerializeField]
    internal playerInputs player;
    [SerializeField]
    public GameObject prefab;
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

    private void Attack()
    {
        //if(inRange(Player))
            //player.TakeDamage(attackDamage);

    }

    // Start is called before the first frame update
    void Start()
    {
        Health = 50;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
