using UnityEngine;
using System.Collections;
using UnityEditor;

public class ProjectileController : MonoBehaviour
{
    // Object that instantiated projectile
    public ShootingController Player;
    public float Speed;

	// Use this for initialization
    private void Start()
    {
        Destroy(gameObject, 5.0f); // Destroy after 5 seconds
        
        Damage = 10.0f;

        // Say whether enemy shot or friendly shot
        if (Player.CompareTag("Player"))
        {
            gameObject.tag = "PlayerLemon";
        }
        else
            gameObject.tag = "EnemyLemon";
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
