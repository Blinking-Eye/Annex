using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
    // Object that instantiated projectile
    public ShootingController Player;
    private Rigidbody2D _rb;

	// Use this for initialization
    private void Start()
    {
        Destroy(gameObject, 5.0f); // Destroy after 5 seconds

        _rb = GetComponent<Rigidbody2D>();

        // Set damage based on power of shot
        if (Player.Power >= 70.0f)
            Damage = 30.0f;
        else
            Damage = 10.0f;

        // Say whether enemy shot or friendly shot
        if (Player.CompareTag("Player"))
            gameObject.tag = "PlayerProjectile";
        else
            gameObject.tag = "EnemyProjectile";
    }

    // Update is called once per frame
    private void Update()
    {
        transform.LookAt(transform.position - (Vector3)_rb.velocity.normalized);
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        // Destroy upon collision
        Destroy(gameObject);
    }

    /* Getters and Setters */

    // Damage
    public float Damage { get; set; }
}
