using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    /* Movement */
    [SerializeField] private float MaxSpeed = 15.0f;
    [SerializeField] private float DashVelocity = 100.0f;

    /* Wall Jumps */
    private bool _touchingWall = false;
    private bool _justWallJumped = false;
    private const float WallRadius = 0.42f;
    [SerializeField] private float WallJumpPush = 10.0f; // The force that pushes you off the wall
    private int _pushCount = 0;
    [SerializeField] private int _waitTimer = 5;
    [SerializeField] private Transform WallCheck;
    [SerializeField] private LayerMask DefineWall;

    /* Falling and Jumping */
    private bool _grounded = false; // Are you on the ground?
    [SerializeField] private float Jump = 800.0f;
    private bool _canDoubleJump = false;
    [SerializeField] private float DoubleJump = 600.0f;
    [SerializeField] private float FallSpeed = 10.0f;
    private const float GroundRadius = 0.2f; // How big will the sphere be that we check for ground?
    [SerializeField] private Transform GroundCheck; // Creates another object to say where the ground should be
    [SerializeField] private LayerMask DefineGround; // Used to say what IS ground
    
    private Rigidbody2D _rb; // Define the object to work on
    private Animator _anim; // Animator

    void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();

        // Continuous collision detection for high speed
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        FacingRight = true;
        Dashing = false;
    }
    
    void Update()
    {
        bool justJumped = false; // To correct for applying jump and wall jump

        // To allow pushing off wall
        if (_pushCount < _waitTimer && _justWallJumped)
            _pushCount++;
        if (_pushCount >= _waitTimer)
            _justWallJumped = false;

        // Don't do this in the future, do input manager and make jump axis to allow remapping
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.L))
        {
            if (_grounded)
            {
                _anim.SetBool("Ground", false);
                _rb.velocity = new Vector2(_rb.velocity.x, 0.0f);
                _rb.AddForce(new Vector2(0, Jump)); // Jump
                justJumped = true;

                _canDoubleJump = true;
            }
            else if (_canDoubleJump) // Double jump
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 0.0f);
                _rb.AddForce(new Vector2(0, DoubleJump));

                _canDoubleJump = false;
            }
            if (_touchingWall && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !justJumped)
            {
                _anim.SetBool("WallHug", true);

                WallJumpPush = Mathf.Abs(WallJumpPush);
                WallJumpPush = FacingRight ? -WallJumpPush : WallJumpPush;
                _rb.velocity = new Vector2(_rb.velocity.x, 0.0f);
                _rb.AddForce(new Vector2(WallJumpPush, DoubleJump));

                _justWallJumped = true;
                _pushCount = 0;
            }
        }
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !_grounded)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, -FallSpeed);
        }
    }
    
    void FixedUpdate()
    {
        // Checks to see if touching wall
        _touchingWall = Physics2D.OverlapCircle(WallCheck.position, WallRadius, DefineWall);
        _anim.SetBool("WallHug", _touchingWall);
        
        if (_touchingWall)
        {
            _grounded = false;
            _canDoubleJump = false;
        }

        /* Check if on ground; first arg is where circle check is generated, second is size,
        third is all things it will collide with. If true, on ground. */
        _grounded = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, DefineGround);
        _anim.SetBool("Ground", _grounded); // Set animation value "Ground" based on bool

        _anim.SetFloat("vSpeed", _rb.velocity.y); // How fast you are moving up and down

        if (Input.GetKeyDown(KeyCode.LeftShift) && _grounded) // Dash
        {
            Debug.Log("Dashing");
            Dashing = true;

            DashVelocity = Mathf.Abs(DashVelocity);
            _rb.velocity = Vector2.zero;
            DashVelocity = FacingRight ? DashVelocity*100 : DashVelocity*-100;
            _rb.AddForce(new Vector2(DashVelocity*Time.fixedDeltaTime, 0.0f));
        }
        else if (!_justWallJumped) // After wall jump, don't allow control
        {
            Dashing = false;

            float move = Input.GetAxis("Horizontal");

            _anim.SetFloat("Speed", Mathf.Abs(move)); // How fast you are moving left and right

            _rb.velocity = new Vector2(move*MaxSpeed, _rb.velocity.y);

            if (move > 0 && !FacingRight)
                Flip();
            else if (move < 0 && FacingRight)
                Flip();
        }

        _anim.SetBool("Dashing", Dashing);

        // If moving up, don't allow collisions with platforms, else allow (ie downward)
        if (_rb.velocity.y > 0.0f || !_grounded)
            Physics2D.IgnoreLayerCollision(8, 9, true);
        else
            Physics2D.IgnoreLayerCollision(8, 9, false);
    }

    // Flips animations if changing direction.
    // Do NOT use if main camera under player heirarchy
    public void Flip()
    {
        FacingRight = !FacingRight; // Flip
        Vector3 scale = transform.localScale; // Get scale of object

        // Flip and apply
        scale.x *= -1;
        transform.localScale = scale;
    }

    /* Getters and Setters */

    // Direction of game object
    public bool FacingRight { get; set; }

    public bool Dashing { get; set; }
}
