using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisonProjectile : ProjectileManager
{

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bounds")
            Destroy(gameObject);
        else if (collision.gameObject.tag == "Enemy")
        {
            HitCharacters(new List<Collider2D> { collision });
            Destroy(gameObject);
        }
    }
}
