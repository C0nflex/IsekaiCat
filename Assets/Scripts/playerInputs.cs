using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class playerInputs : MonoBehaviour
{
    public static playerInputs instance;
    [Header("RUN")]
    [SerializeField] public float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float velPower;
    [SerializeField] private float frictionAmount;
    [Header("JUMP")]
    [SerializeField] private float accelerationInAir;
    [SerializeField] private float deccelerationInAir;
    [SerializeField] private float fallGravityMultiplier;
    [SerializeField] private float jumpCoyoteTime;
    [SerializeField] private float jumpHangTimeThreshold;
    [SerializeField] private float jumpHangGravityMultiplier;
    [Header("CHECKS")]
    [SerializeField] private BoxCollider2D groundcheck;
    [SerializeField] LayerMask groundMask;
    [Header("CAMERA")]
    [SerializeField] protected LayerMask EnemyLayer;
    [SerializeField] protected float basicAttackDamage;
    [SerializeField] protected Vector2 basicAttackKnockback;
    private cameraFollowObject _cameraFollowObject;
    protected Rigidbody2D _rigidBody;
    private Animator anim;
    public bool IsFacingRight = true;
    private float _fallSpeedYDampingChangeThreshold;
    private bool isJumping = false;

    private Transform stepPos;
    private Transform wallPos;

    private bool bounce = false;
    //private float moveSpeed = 0.2f; // Horizontal movement speed in units per second
    private float moveDuration = 0.2f; // Duration of movement in seconds
    protected bool isMidJump = false;
    private float gravityScale;  
    private float xInput;
    protected float lastGroundedTime;
    protected bool isGoingUpStairs = false;


    private void Awake()
    {
        instance = this;
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        gravityScale = _rigidBody.gravityScale;
    }

    private void Start()
    {
        _cameraFollowObject = GameManager.Instance._cameraFollowObject;
        _fallSpeedYDampingChangeThreshold = cameraManger.Instance._fallSpeedYDamoingChangeThreshold;
        stepPos = transform.Find("TileStepCheck").transform;
        wallPos = transform.Find("WallStepCheck").transform;
        _cameraFollowObject.NewObjectToFollow(transform);
    }

    void Update()
    {
        GetInput();
        //CheckStep();
        HandleJump();
        HandleAbilites();
        anim.SetFloat("speed", Mathf.Abs(xInput));

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
        {
            _rigidBody.gravityScale = gravityScale * jumpHangGravityMultiplier;
        }
        else if (_rigidBody.velocity.y < 0)
        {
            _rigidBody.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else
        {
            _rigidBody.gravityScale = gravityScale;
        }
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

    private void HandleAbilites()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            BasicAttack();
        if (Input.GetKeyDown(KeyCode.Mouse1))
            RangedAttack();
    }

    private void GetInput()
    {
        xInput = Input.GetAxis("Horizontal");
    }

    private void MoveWithInput()
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
        anim.SetBool("grounded", Physics2D.OverlapAreaAll(groundcheck.bounds.min, groundcheck.bounds.max, groundMask).Length > 0);
        if (anim.GetBool("grounded"))
        {
            lastGroundedTime = jumpCoyoteTime;
            isMidJump = false;
        }
        else isMidJump = true;
    }
    /*
    private void CheckStep()
    {
        //Tilemap col = GameManager.instance.groundTilemap.GetComponent<Tilemap>();
        var StepcircleBound = Physics2D.OverlapCircle(stepPos.position, 0.001f, groundMask);
        var WallCircleBound = Physics2D.OverlapCircle(wallPos.position, 0.001f, groundMask);
        if (StepcircleBound != null && WallCircleBound ==null && !bounce)
        {
            Vector2 currentPosition = transform.position;
            float zRotation = transform.rotation.eulerAngles.z;
            var roation = -1;
            if (zRotation < 0) { roation = 1; }
            var xmove = 0.2f * roation;
            Vector2 newPosition = new Vector2(currentPosition.x + xmove, currentPosition.y + 0.3f);
            transform.position = newPosition;
            bounce = true;
        }
        else
        {
            bounce = false;
        }
    } */

    private void CheckStep()
    {
        var StepcircleBound = Physics2D.OverlapCircle(stepPos.position, 0.01f, groundMask);
        var WallCircleBound = Physics2D.OverlapCircle(wallPos.position, 0.01f, groundMask);

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
        if ((zRotation % 360) >0) { roation = -1; }
        float targetX = currentPosition.x + (0.2f*roation); // Move 0.2 units to the right
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
    protected virtual void Jump()
    {
        lastGroundedTime = 0;
    }

    protected abstract void HandleJump();

    protected abstract void BasicAttack();

    protected abstract void RangedAttack();

}
