using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class playerInputs : MonoBehaviour
{
    [Header("RUN") ]
    [SerializeField] public float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float velPower;
    [SerializeField] private float frictionAmount;
    [Header("JUMP")]
    [SerializeField] public float jumpspeed;
    [SerializeField] private float accelerationInAir;
    [SerializeField] private float deccelerationInAir;
    [SerializeField] private float fallGravityMultiplier;
    [SerializeField] private float jumpCoyoteTime;
    [SerializeField] private float jumpCutMultiplier;
    [SerializeField] private float jumpHangTimeThreshold;
    [SerializeField] private float jumpHangGravityMultiplier;
    [Header("CHECKS")]
    [SerializeField] private BoxCollider2D groundcheck;
    [SerializeField] LayerMask groundMask;
    [Header("CAMERA")]
    [SerializeField] private GameObject _cameraFollowGO;
    private cameraFollowObject _cameraFollowObject;
    private Rigidbody2D _rigidBody;
    private Animator anim;
    public bool IsFacingRight = true;
    private float _fallSpeedYDampingChangeThreshold;
    private float lastGroundedTime;
    private bool isJumping = false;
    private float gravityScale;  
    private float xInput;



    private void Awake()
    {
        _cameraFollowObject = _cameraFollowGO.GetComponent<cameraFollowObject>();
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        gravityScale = _rigidBody.gravityScale;
    }

    private void Start()
    {
        _fallSpeedYDampingChangeThreshold =  cameraManger.Instance._fallSpeedYDamoingChangeThreshold;
    }

    void Update()
    {
        GetInput();
        HandleJump();    
        anim.SetFloat("speed", Mathf.Abs(xInput));
 
        if (_rigidBody.velocity.y < _fallSpeedYDampingChangeThreshold && !cameraManger.Instance.isLerpingYDamping && !cameraManger.Instance.LerpedFromPlayerFalling)
        {
            cameraManger.Instance.LerpYDamping(true);
        }

        if(_rigidBody.velocity.y >= 0f && !cameraManger.Instance.isLerpingYDamping && !cameraManger.Instance.LerpedFromPlayerFalling)
        {
            cameraManger.Instance.LerpedFromPlayerFalling = false;

            cameraManger.Instance.LerpYDamping(false);
        }

        #region Timer
        lastGroundedTime -= Time.deltaTime;;
        #endregion

        if(Mathf.Abs(_rigidBody.velocity.y) < jumpHangTimeThreshold)
        {
            _rigidBody.gravityScale = gravityScale * jumpHangGravityMultiplier;
        }
        else if(_rigidBody.velocity.y < 0)
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
        ApplyFriction();
        MoveWithInput();

        if(xInput != 0)
        {
            TurnCheck();
        }
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

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && lastGroundedTime > 0 && !isJumping)
        {
            _rigidBody.AddForce(Vector2.up * jumpspeed, ForceMode2D.Impulse);
            lastGroundedTime = 0;
        }
        
        if(Input.GetKeyUp(KeyCode.Space) && _rigidBody.velocity.y > 0 )
        {
            _rigidBody.AddForce(Vector2.down * _rigidBody.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }
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
            isJumping = false;
        }
        else isJumping = true;
    }

    private void TurnCheck()
    {
        if(xInput > 0 && !IsFacingRight)
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
        if(IsFacingRight)
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
    

}
