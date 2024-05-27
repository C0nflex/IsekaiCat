using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SwoardInputs : playerInputs
{
    private float lastfloatTimer = Mathf.Infinity;
    public float currFloatAmount = 1;
    private float orgvelocity;
    [SerializeField] private Image FlightBar;
    [SerializeField] private Collider2D topCollider;
    [SerializeField] private float floatingSpeed;
    [SerializeField] private float cooldownForBarRegain;
    [SerializeField] private float floatBarDecreaseSpeed;
    [SerializeField] private float floatBarIncreaseSpeed;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius;
    [Header("Dash")]
    [SerializeField] private bool isDashing;
    [SerializeField] private float dashingSpeed = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingDamage;
    [SerializeField] private Vector2 dashingKnockback;
    [SerializeField] private TrailRenderer GeneralTrailRenderer;
    private TrailRenderer DashTrailRenderer;
    public List<Collider2D> enemiesAlreadyHitByDash = new List<Collider2D>();

    public AudioClip swordSlash;
    public AudioClip swordDash;


    protected override void Start()
    {
        DashTrailRenderer = GetComponent<TrailRenderer>();
        gravityScale = _rigidBody.gravityScale;
        base.Start();
    }
    protected override void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currFloatAmount > 0)
            StartFloat();
        if (Input.GetKey(KeyCode.Space) && currFloatAmount > 0)
            Float();
        else
            _rigidBody.gravityScale = gravityScale;
        if (lastfloatTimer > cooldownForBarRegain)
            UpdateFloatAmount(floatBarIncreaseSpeed * Time.deltaTime);
    }

    protected override void MoveWithInput()
    {
        base.MoveWithInput();
    }

    protected override void InFallTime()
    {

    }

    protected override void InHangTime()
    {

    }

    protected override void ResetGravity()
    {
        
    }

    protected override void Update()
    {
        if (isDashing)
        {
            anim.SetBool("dashing", true);
            
            List<Collider2D> enemiesHit = new List<Collider2D>();
            foreach (LayerMask EnemyLayer in EnemyLayers)
            {
                enemiesHit.AddRange(Physics2D.OverlapCircleAll(transform.position, attackRadius, EnemyLayer));
            }
            enemiesHit = enemiesHit.Where(x => !enemiesAlreadyHitByDash.Contains(x)).ToList();
            foreach (var collider in enemiesHit)
                if (isDashing && collider.gameObject.tag == "Enemy")
                {
                    collider.gameObject.GetComponent<Health>().TakeDamage(dashingDamage, dashingKnockback, gameObject);
                    if(!enemiesAlreadyHitByDash.Contains(collider))
                        enemiesAlreadyHitByDash.Add(collider);
                }
        }
        lastfloatTimer += Time.deltaTime;
        FlightBar.fillAmount = currFloatAmount;
        if(!isDashing)
        {
            anim.SetBool("dashing", false);
        }
        base.Update();
    }

    private void StartFloat()
    {
        _rigidBody.gravityScale = 0;
        _rigidBody.velocity = new Vector3(0, floatingSpeed ,0);
        //_rigidBody.AddForce(new Vector3(0, 1, 0) * floatingSpeed,ForceMode2D.Impulse);
        //.position += new Vector3(0,1,0) * floatingSpeed * Time.deltaTime;
    }

    private void Float()
    {
        Debug.Log(_rigidBody.velocity);
        if (_rigidBody.velocity.y < floatingSpeed && Physics2D.OverlapAreaAll(topCollider.bounds.min, topCollider.bounds.max, groundMasks[0]).Length == 0)
        {
            _rigidBody.velocity = new Vector3(0, floatingSpeed, 0);
        }
        //_rigidBody.velocity = new Vector3(_rigidBody.velocity.x, 0, 0);
        //transform.position += new Vector3(0, 1, 0) * floatingSpeed * Time.deltaTime;
        UpdateFloatAmount(-1 * floatBarDecreaseSpeed * Time.deltaTime);
        lastfloatTimer = 0;
    }

    private void UpdateFloatAmount(float amount) => currFloatAmount = Mathf.Min(Mathf.Max(currFloatAmount + amount, 0), 1);

    protected override void Jump() //Swords dont jump!
    {
        
    }

    protected override void BasicAttack()
    {
        anim.SetTrigger("slash");
        List<Collider2D> enemiesHit = new List<Collider2D>();
        foreach (LayerMask EnemyLayer in EnemyLayers)
        {
            enemiesHit.AddRange(Physics2D.OverlapCircleAll(transform.position, attackRadius, EnemyLayer));
        }
        foreach (Collider2D enemy in enemiesHit)
        {
            if (enemy.tag == "Enemy")
                enemy.GetComponent<Health>().TakeDamage(basicAttackDamage, basicAttackKnockback, gameObject);
            if (enemy.name == "Ogre")
            {
                Health health = enemy.GetComponent<Health>();
                if(!health._Dead)
                audioSource.PlayOneShot(OgreHit);
                else
                audioSource.PlayOneShot(OgreDie);
            }
            else if (enemy.name == "Bat")
            {
                Health health = enemy.GetComponent<Health>();
                if (!health._Dead)
                    audioSource.PlayOneShot(BatHit);
                else
                    audioSource.PlayOneShot(BatDie);
            }


        }
        audioSource.PlayOneShot(swordSlash);
        base.BasicAttack();
    }

    protected override void RangedAttack()
    {
        StartCoroutine(Dash());
        base.RangedAttack();
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        audioSource.PlayOneShot(swordDash);
        float originalGravity = _rigidBody.gravityScale;
        _rigidBody.gravityScale = 0f;
        _rigidBody.velocity = new Vector2((IsFacingRight ? 1 : -1) * dashingDamage, 0f);
        GeneralTrailRenderer.emitting = false;
        DashTrailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        _rigidBody.gravityScale = originalGravity;
        _rigidBody.velocity = Vector2.zero;
        DashTrailRenderer.emitting = false;
        GeneralTrailRenderer.emitting = true;
        enemiesAlreadyHitByDash = new List<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }


}
