using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class ShootingController : MonoBehaviour
{

    [SerializeField] private GameObject Lemon;
    private bool _charging = false;
    private float _chargeTimer = 1.0f;
    [SerializeField] private float FireRate = 0.4f;
    private float _fireNext = 0.0f;
    [SerializeField] private float Speed;

    private Animator _anim;
    private GameObject _projectile;
    private Rigidbody2D _rb;

    // Use this for initialization
    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _anim.SetBool("Charging", _charging);
        _anim.SetBool("Shooting", false);

        FacingRight = gameObject.GetComponent<PlayerController>().FacingRight;
        if (Time.fixedTime > _fireNext)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _charging = true;
                _chargeTimer += Time.deltaTime;
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

                // Instantiate projectile and give velocity
                _projectile = (GameObject) Instantiate(Lemon, myPos, Quaternion.identity);
                _rb = _projectile.GetComponent<Rigidbody2D>();
                _projectile.GetComponent<ProjectileController>().Player = this;

                int rotate = FacingRight ? -90 : 90;
                _projectile.transform.Rotate(0, 0, rotate);
                _rb.velocity = FacingRight ? new Vector2(Speed, 0) : new Vector2(-Speed, 0);
                
                _chargeTimer = 1.0f;
            }
        }
    }

    /* Getters and Setters */

    public bool FacingRight { get; set; }
}