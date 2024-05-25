using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float damageToDeal;
    private Vector2 knockbackToDeal;

    public void Init(Vector3 direction, float speed, float damageToDeal,Vector2 knockbackToDeal)
    {
        this.direction = direction;
        this.speed = speed;
        this.damageToDeal = damageToDeal;
        this.knockbackToDeal = knockbackToDeal;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bounds")
            Destroy(gameObject);
        else if (collision.gameObject.tag == "Enemy")
        {
             collision.GetComponent<Health>().TakeDamage(damageToDeal, knockbackToDeal, gameObject);
            Destroy(gameObject);
        }
    }
}
