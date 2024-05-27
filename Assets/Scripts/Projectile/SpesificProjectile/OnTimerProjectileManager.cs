using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OnTimerProjectileManager : ProjectileManager
{
    [SerializeField] private float timeToWaitForExplosion;
    [SerializeField] private float explosionRadius;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private bool dropoffKnockbackBasedOnDistance;
    private Rigidbody2D m_rigidbody2D;

    private void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        StartCoroutine(WaitForExplosion());
        m_rigidbody2D.AddForce(direction * speed, ForceMode2D.Impulse);
    }

    private IEnumerator WaitForExplosion()
    {
        yield return new WaitForSeconds(timeToWaitForExplosion);
        Explode();
    }

    private void Explode()
    {
        List<Collider2D> charactersHit = new List<Collider2D>();
        charactersHit.AddRange(Physics2D.OverlapCircleAll(transform.position, explosionRadius, layerMask));
        var knockback = knockbackToDeal;
        if (dropoffKnockbackBasedOnDistance)
            HitCharacters(charactersHit);
        else
            base.HitCharacters(charactersHit);
        Destroy(gameObject);
    }

    protected override void HitCharacters(List<Collider2D> Characters)
    {
        var UniqueColliders = Characters.Select(x => x.gameObject).ToHashSet().Select(x => x.GetComponent<Collider2D>());
        foreach (Collider2D character in UniqueColliders)
        {
            if (character.GetComponent<Health>() != null)
                character.GetComponent<Health>().TakeDamage(damageToDeal, knockbackToDeal * (1 - Vector3.Distance(character.transform.position, transform.position) / explosionRadius), gameObject);
            else if (character.transform.parent != null && character.transform.parent.GetComponent<Health>() != null)
                character.transform.parent.GetComponent<Health>().TakeDamage(damageToDeal, knockbackToDeal * (1 - Vector3.Distance(character.transform.parent.transform.position, transform.position) / explosionRadius), gameObject);
        }
    }


    protected override void FixedUpdate()
    {
        
    }
}
