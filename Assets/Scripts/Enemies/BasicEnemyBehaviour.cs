using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using TMPro;

public class BasicEnemyBehaviour : MonoBehaviour
{
    public enum Direction { Left, Right };
    private const float COOLDOWN = 1f; // attack cooldown 1 second
    [SerializeField]
    private GameObject player;
    private GameObject prefab;
    private TextMeshProUGUI healthText;
    private int attackDamage;
    private Direction facingDirection;
    private Health health;
    
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
        if (math.abs(transform.position.x - player.transform.position.x) < 0.8)
            player.GetComponent<Health>().TakeDamage(attackDamage, new Vector2(0f,0f), gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        health = gameObject.GetComponent<Health>();
        prefab = gameObject;
        attackDamage = 20;
        StartCoroutine(AttackEverySecond());
        healthText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        healthText.text = health._currentHealth.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), 1f * Time.deltaTime);
        healthText.text = health._currentHealth.ToString();
    }
}
