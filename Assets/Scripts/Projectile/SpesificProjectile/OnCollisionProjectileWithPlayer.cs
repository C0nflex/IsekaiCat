using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionWithPlayer : ProjectileManager
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bounds")
            Destroy(gameObject);
        else if (collision.gameObject.tag == "Player")
        {
            playerInputs.Instance.GetComponent<Health>().TakeDamage(damageToDeal, knockbackToDeal, gameObject);
            Destroy(gameObject);
        }
    }
}
