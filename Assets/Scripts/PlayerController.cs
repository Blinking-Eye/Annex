using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    /* Movement */
    [SerializeField] float MaxSpeed = 15.0f;

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
    }

    // Called each frame
    void Update()
    {   
        // Don't do this in the future, do input manager and make jump axis to allow remapping
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.L))
        {
            if (_grounded)
            {
                _anim.SetBool("Ground", false);
                _rb.velocity = new Vector2(_rb.velocity.x, 0.0f);
                _rb.AddForce(new Vector2(0, Jump)); // Jump

                _canDoubleJump = true;
            }
            else if (_canDoubleJump) // Double jump
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 0.0f);
                _rb.AddForce(new Vector2(0, DoubleJump));

                _canDoubleJump = false;
            }
        }
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !_grounded)
        {
            if (_rb.velocity.y >= 0.0f)
                _rb.velocity = new Vector2(_rb.velocity.x, -FallSpeed);
            else
                _rb.velocity = new Vector2(_rb.velocity.x, -FallSpeed);
        }
    }

    // Called before any physics calculations
    void FixedUpdate()
    {
    
        /* Check if on ground; first arg is where circle check is generated, second is size,
        third is all things it will collide with. If true, on ground. */
        _grounded = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, DefineGround);
        _anim.SetBool("Ground", _grounded); // Set animation value "Ground" based on bool

        _anim.SetFloat("vSpeed", _rb.velocity.y); // How fast you are moving up and down

        float move = Input.GetAxis("Horizontal");

        _anim.SetFloat("Speed", Mathf.Abs(move)); // How fast you are moving left and right

        _rb.velocity = new Vector2(move * MaxSpeed, _rb.velocity.y);

        if (move > 0 && !FacingRight)
            Flip();
        else if (move < 0 && FacingRight)
            Flip();
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
}
