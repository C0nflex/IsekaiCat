using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public abstract class playerInputs : MonoBehaviour
{
    public static playerInputs Instance;
    [Header("RUN")]
    [SerializeField] public float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float velPower;
    [SerializeField] private float frictionAmount;
    [Header("JUMP")]
    [SerializeField] private float jumpCooldown;
    protected bool jumpOnCooldown = false;
    [SerializeField] private float accelerationInAir;
    [SerializeField] private float deccelerationInAir;
    [SerializeField] private float fallGravityMultiplier;
    [SerializeField] private float jumpCoyoteTime;
    [SerializeField] private float jumpHangTimeThreshold;
    [SerializeField] private float jumpHangGravityMultiplier;
    [Header("CHECKS")]
    [SerializeField] private BoxCollider2D groundcheck;
    [SerializeField] protected List<LayerMask> groundMasks;
    [Header("CAMERA")]
    [SerializeField] protected List<LayerMask> EnemyLayers;
    [SerializeField] protected float basicAttackDamage;
    [SerializeField] protected Vector2 basicAttackKnockback;
    [Header("Abilities")]
    [SerializeField] private float basicAttackCooldown;
    [SerializeField] private float rangedAttackCooldown;
    private bool basicAttackOnCooldown = false;
    private bool rangedAttackOnCooldown = false;
    private cameraFollowObject _cameraFollowObject;
    protected Rigidbody2D _rigidBody;
    protected Animator anim;
    public bool IsFacingRight = true;
    private float _fallSpeedYDampingChangeThreshold;

    private Transform stepPos;
    private Transform wallPos;

    private bool bounce = false;
    //private float moveSpeed = 0.2f; // Horizontal movement speed in units per second
    private float moveDuration = 0.2f; // Duration of movement in seconds
    protected bool isMidJump = false;
    protected float gravityScale;
    private float xInput;
    protected float lastGroundedTime;
    protected bool isGoingUpStairs = false;
    public bool canMove = false;


    public ParticleSystem rangedAttackParticles;
    private Vector2 startPos; // for player restart position

    private bool isGrounded = false;

    public bool isCatAtEnd = false;
    public bool isVendingAtEnd = false;
    public bool isSwordAtEnd = false;
    public bool isDead = false;
    //public GameObject isDeadText;
    public GameObject respawnPre;
    public int SoulLevel; //TODO:  temp value 
    public AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        EnemyLayers.Add(LayerMask.GetMask("Enemy"));
        EnemyLayers.Add(LayerMask.GetMask("Bat"));
        gravityScale = _rigidBody.gravityScale;

    }

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _cameraFollowObject = GameManager.Instance._cameraFollowObject;
        _fallSpeedYDampingChangeThreshold = cameraManger.Instance._fallSpeedYDamoingChangeThreshold;
        stepPos = transform.Find("TileStepCheck").transform;
        wallPos = transform.Find("WallStepCheck").transform;
        startPos = transform.position;
        _cameraFollowObject.NewObjectToFollow(transform);
        SoulLevel = 0;
        
    }

    protected virtual void Update()
    {
        anim.SetFloat("speed", Mathf.Abs(xInput));
        if (isDead)
        {
            //gameObject.SetActive(false);

            respawnPre.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                isDead = false;
                respawnPre.SetActive(false);
                gameObject.transform.position = startPos;
                gameObject.GetComponent<Health>().UpdateHPToMax();
                //gameObject.anima.SetTrigger("die");
                gameObject.GetComponent<Animator>().SetTrigger("respawn");
                //Instantiate(a, startPos , Quaternion.identity);
            }
        }
        //else { isDeadText.SetActive(false); }
        else if (canMove)
        {
            
            GetInput();
            //CheckStep();
            HandleJump();
            HandleAbilites();
            PlayerRestartGame();
            PlayerRestartPosition();
            //anim.SetFloat("speed", Mathf.Abs(xInput));

            if (_rigidBody.velocity.y < _fallSpeedYDampingChangeThreshold && !cameraManger.Instance.isLerpingYDamping && !cameraManger.Instance.LerpedFromPlayerFalling)
            {
                cameraManger.Instance.LerpYDamping(true);
            }

            if (_rigidBody.velocity.y >= 0f && !cameraManger.Instance.isLerpingYDamping && !cameraManger.Instance.LerpedFromPlayerFalling)
            {
                cameraManger.Instance.LerpedFromPlayerFalling = false;

                cameraManger.Instance.LerpYDamping(false);
            }

            #region Timer
            lastGroundedTime -= Time.deltaTime; ;
            #endregion

            if (Mathf.Abs(_rigidBody.velocity.y) < jumpHangTimeThreshold)
                InHangTime();
            else if (_rigidBody.velocity.y < 0)
                InFallTime();
            else
                ResetGravity();
        }
    }

    protected virtual void ResetGravity() => _rigidBody.gravityScale = gravityScale;

    protected virtual void InHangTime()
    {
        _rigidBody.gravityScale = gravityScale * jumpHangGravityMultiplier;
    }

    protected virtual void InFallTime()
    {
        _rigidBody.gravityScale = gravityScale * fallGravityMultiplier;
    }

    private void FixedUpdate()
    {

        CheckGround();
        //CheckStep();
        ApplyFriction();
        MoveWithInput();

        if (xInput != 0)
        {
            TurnCheck();
        }

    }


    public void EnableMovement()
    {
        canMove = true;
    }

    // Function to disable player movement
    public void DisableMovement()
    {
        canMove = false;
    }

    private void HandleAbilites()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !basicAttackOnCooldown)
        {
            BasicAttack();
            anim.SetTrigger("melee");
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && !rangedAttackOnCooldown)
            RangedAttack();
    }

    private void PlayerRestartGame()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }



    public void PlayerRestartPosition()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            transform.position = startPos;
        }
    }

    private void GetInput()
    {
        xInput = Input.GetAxis("Horizontal");
    }

    protected virtual void MoveWithInput()
    {
        float targetSpeed = xInput * speed;

        float speedDif = targetSpeed - _rigidBody.velocity.x;

        float accelRate;

        if (lastGroundedTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0) ? acceleration : decceleration;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0) ? accelerationInAir : deccelerationInAir;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        _rigidBody.AddForce(movement * Vector2.right);
    }

    private void ApplyFriction()
    {
        if (anim.GetBool("grounded") && xInput == 0 && _rigidBody.velocity.y <= 0)
        {
            float amount = Mathf.Min(Mathf.Abs(_rigidBody.velocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(_rigidBody.velocity.x);

            _rigidBody.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }

    private void CheckGround()
    {
        foreach (var groundMask in groundMasks)
            if (Physics2D.OverlapAreaAll(groundcheck.bounds.min, groundcheck.bounds.max, groundMask).Length > 0)
                Grounded();
            else
            {
                isGrounded = false;
                anim.SetBool("grounded", false);
            }

        if (isGrounded)
        {
            lastGroundedTime = jumpCoyoteTime;
            isMidJump = false;
            
        }
        else
            isMidJump = true;
    }

    protected virtual void Grounded()
    {
        isGrounded = true;
        anim.SetBool("grounded", true);
    }
    
    private void CheckStep()
    {
        var StepcircleBound = Physics2D.OverlapCircle(stepPos.position, 0.01f, groundMasks[0]);
        var WallCircleBound = Physics2D.OverlapCircle(wallPos.position, 0.01f, groundMasks[0]);

        if (StepcircleBound != null && WallCircleBound == null && !bounce)
        {
            StartCoroutine(MovePlayerCoroutine()); // Start rotating the player
            bounce = true;
        }
        else
        {
            bounce = false;
        }
    }

    private IEnumerator MovePlayerCoroutine()
    {
        Vector2 currentPosition = transform.position;
        float zRotation = transform.rotation.eulerAngles.y;
        var roation = 1;
        if ((zRotation % 360) > 0) { roation = -1; }
        float targetX = currentPosition.x + (0.2f * roation); // Move 0.2 units to the right
        float targetY = currentPosition.y + 0.20f; // and thhis
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGoingUpStairs = true;
                break;
            }
            // Interpolate between current and target positions
            float newX = Mathf.Lerp(currentPosition.x, targetX, elapsedTime / moveDuration);
            float newY = Mathf.Lerp(currentPosition.y, targetY, elapsedTime / moveDuration);
            transform.position = new Vector2(newX, newY);


            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure the player reaches the target position exactly
        transform.position = new Vector2(targetX, targetY);
        isGoingUpStairs = false;
    }

    public void setPlayerCheckPoint(Vector2 flagPoint)
    {
        startPos = flagPoint;
    }

    private void TurnCheck()
    {
        if (xInput > 0 && !IsFacingRight)
        {
            Turn();
        }
        else if (xInput < 0 && IsFacingRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        if (IsFacingRight)
        {
            Vector3 rotator = new Vector3(0f, 180f, 0f);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;

            _cameraFollowObject.CallTurn();
        }
        else
        {
            Vector3 rotator = new Vector3(0f, 0f, 0f);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;

            _cameraFollowObject.CallTurn();
        }
    }

    private void ExitBall()
    {
        Vector3 newPos = transform.position;
        newPos.y += 0.2f;
        transform.position = newPos;
    }

    protected virtual void Jump()
    {
        lastGroundedTime = 0;
        isMidJump = true;
        StartCoroutine(JumpCooldown());
    }


    protected abstract void HandleJump();

    protected virtual void BasicAttack()
    {
        StartCoroutine(BasicAttackCooldown());
    }

    protected virtual void RangedAttack()
    {
        StartCoroutine(RangedAttackCooldown());
    }

    protected IEnumerator JumpCooldown()
    {
        jumpOnCooldown = true;
        yield return new WaitForSeconds(jumpCooldown);
        jumpOnCooldown = false;
    }

    protected IEnumerator BasicAttackCooldown()
    {
        basicAttackOnCooldown = true;
        yield return new WaitForSeconds(basicAttackCooldown);
        basicAttackOnCooldown = false;
    }

    protected IEnumerator RangedAttackCooldown()
    {
        if(rangedAttackParticles != null)
            rangedAttackParticles.gameObject.SetActive(false);
        rangedAttackOnCooldown = true;
        yield return new WaitForSeconds(rangedAttackCooldown);
        rangedAttackOnCooldown = false;
        if (rangedAttackParticles != null)
            rangedAttackParticles.gameObject.SetActive(true);
    }
    public string returnPlayerName()
    {
        return gameObject.name;
    }
    public void catAtEnd()
    {
        isCatAtEnd = true;
    }
    public void VedingAtEnd()
    {
        isVendingAtEnd = true;
    }
    public void swordAtEnd()
    {
        isSwordAtEnd = true;
    }

    public bool isEnoughSoul() //TODO:  temp value 
    {
        if(SoulLevel>0)
        {
            return true;
        }
        return false;
    }

    public void updateStartPos(Vector2 newStartPos)
    {
        startPos = newStartPos;
    }
    public void increaseSoulLevel(int level)
    {
        SoulLevel += level;
    }

    public int returnPlayerSoul()
    {
        return SoulLevel;
    }

}
