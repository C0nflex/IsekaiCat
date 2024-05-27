using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    protected float projectileGravity;
    protected Vector3 direction;
    protected float speed;
    protected float damageToDeal;
    protected Vector2 knockbackToDeal;

    public void Init(Vector3 direction, float speed, float damageToDeal,Vector2 knockbackToDeal, float projectileGravity)
    {
        this.direction = direction;
        this.speed = speed;
        this.damageToDeal = damageToDeal;
        this.knockbackToDeal = knockbackToDeal;
        this.projectileGravity = projectileGravity;
        GetComponent<Rigidbody2D>().gravityScale = projectileGravity;
    }
    public void Init(Vector3 direction, float speed, float damageToDeal, Vector2 knockbackToDeal, float projectileGravity, bool isFacingRight)
    {
        this.direction = direction;
        this.speed = speed;
        this.damageToDeal = damageToDeal;
        this.knockbackToDeal = knockbackToDeal;
        this.projectileGravity = projectileGravity;
        float facingDirection = isFacingRight ? 0 : 180;
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y + facingDirection, transform.rotation.z));

        GetComponent<Rigidbody2D>().gravityScale = projectileGravity;
    }
    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    protected virtual void HitCharacters(List<Collider2D> Characters)
    {
        var UniqueColliders = Characters.Select(x => x.gameObject).ToHashSet().Select(x => x.GetComponent<Collider2D>());
       foreach (Collider2D character in UniqueColliders)
            if(character.GetComponent<Health>() != null)
                character.GetComponent<Health>().TakeDamage(damageToDeal, knockbackToDeal, gameObject);
    }
}
