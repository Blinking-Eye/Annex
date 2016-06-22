using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class ShootingController : MonoBehaviour
{

    [SerializeField] private GameObject Lemon;
    [SerializeField] private GameObject ChargedLemon;
    private bool _charging = false;
    private float _chargeTimer = 1.0f;
    [SerializeField] private float ChargeLimit = 0.0f;
    [SerializeField] private float FireRate = 0.4f;
    private float _fireNext = 0.0f;
    [SerializeField] private float Speed;

    private Animator _anim;
    private GameObject _projectile;
    private Rigidbody2D _rb;
    private Rigidbody2D _rbShooter;

    // Use this for initialization
    private void Start()
    {
        _anim = gameObject.GetComponent<Animator>();
        _rbShooter = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _anim.SetBool("Charging", _charging);
        _anim.SetBool("Shooting", false);
        _anim.SetBool("Charged", false);

        FacingRight = gameObject.GetComponent<PlayerController>().FacingRight;
        if (Time.fixedTime > _fireNext)
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.K))
            {
                _charging = true;
                _chargeTimer += Time.deltaTime;

                if (_chargeTimer > ChargeLimit)
                    _anim.SetBool("Charged", true);
            }
            else if (_charging)
            {
                _anim.SetBool("Shooting", true);
                _charging = !_charging;
                _fireNext = Time.fixedTime + FireRate;

                // Gets position of character
                Vector2 myPos =
                    new Vector2(transform.position.x, transform.position.y);

                // Get direction based off of mouse position and player location
                Vector2 direction = myPos;
                direction.Normalize(); // Normalize

                // Pick bullet type based on how long one held
                GameObject bullet = _chargeTimer < ChargeLimit ? Lemon : ChargedLemon;

                // Instantiate projectile and give velocity
                _projectile = (GameObject) Instantiate(bullet, myPos, Quaternion.identity);
                _rb = _projectile.GetComponent<Rigidbody2D>();
                _projectile.GetComponent<ProjectileController>().Player = this;

                int rotate = FacingRight ? -90 : 90;
                _projectile.transform.Rotate(0, 0, rotate);
                _rb.velocity = FacingRight ? new Vector2(Speed+_rbShooter.velocity.x, 0) :
                                             new Vector2(-Speed+_rbShooter.velocity.x, 0);
                
                _chargeTimer = 1.0f;
            }
        }
    }

    /* Getters and Setters */

    public bool FacingRight { get; set; }
}