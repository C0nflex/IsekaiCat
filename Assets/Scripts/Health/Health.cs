using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public float _startingHealth;
    public float _currentHealth { get; private set; }
    private float _lastHitDmg;
    private Animator _anim;
    public bool _Dead { get; private set; }
    private Rigidbody2D _rd;
    public bool hurt = false;

    // Regeneration settings
    [SerializeField] private float regenRate = 2f; // Health points to regenerate per interval
    [SerializeField] private float regenInterval = 1f; // Time in seconds between each regeneration tick

    private void Awake()
    {
        _Dead = false;
        _currentHealth = _startingHealth;
        _anim = GetComponent<Animator>();
        _rd = GetComponent<Rigidbody2D>();

        // Start the health regeneration coroutine
        StartCoroutine(PassiveRegen());
    }

    private int calculateKnockbackDirection(Vector3 damageDealerPos) =>
        (gameObject.transform.position.x - damageDealerPos.x > 0) ? 1 : -1;

    public void TakeDamage(float damage, Vector2 knockback, GameObject damageDealer)
    {
        int knockbackDirection = calculateKnockbackDirection(damageDealer.transform.position);
        _lastHitDmg = damage;
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _startingHealth);

        if (_currentHealth > 0)
        {
            //_anim.SetTrigger("hurt");
            hurt = true;
            _rd.velocity = new Vector2(_rd.velocity.x + knockback.x * knockbackDirection, _rd.velocity.y + knockback.y);
        }
        else
        {
            if (!_Dead)
            {
                //_anim.SetTrigger("die");
                _Dead = true;
            }
            Destroy(gameObject);
        }
    }

    public void TakeDamageNoAnimation(float damage, Vector2 knockback)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _startingHealth);

        if (_currentHealth > 0)
        {
            hurt = true;
            _rd.velocity = new Vector2(_rd.velocity.x + knockback.x, _rd.velocity.y + knockback.y);
        }
        else
        {
            _Dead = true;
            if (!_Dead)
            {
                if (gameObject.tag == "enemy")
                {
                    //GetComponentInParent<EnemyPatrol>().enabled = false;
                }
            }
        }
    }

    private IEnumerator PassiveRegen()
    {
        while (!_Dead)
        {
            if (_currentHealth < _startingHealth)
            {
                _currentHealth = Mathf.Clamp(_currentHealth + regenRate, 0, _startingHealth);
            }
            yield return new WaitForSeconds(regenInterval);
        }
    }
}
