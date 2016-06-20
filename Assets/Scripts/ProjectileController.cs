using UnityEngine;
using System.Collections;
using UnityEditor;

public class ProjectileController : MonoBehaviour
{
    // Object that instantiated projectile
    public ShootingController Player;
    public float Speed;
    public float Damage = 10.0f; // Default damage for uncharged lemons
    [SerializeField] private float MaxDistance;
    private Vector3 _startPos;

	// Use this for initialization
    private void Start()
    {
        Destroy(gameObject, 5.0f); // Destroy after 5 seconds
        _startPos = transform.position;

        // Say whether enemy shot or friendly shot
        if (Player.CompareTag("Player"))
        {
            gameObject.tag = "PlayerLemon";
        }
        else
            gameObject.tag = "EnemyLemon";
    }

    private void Update()
    {

        // If goes past max distance, destroy
        if (Mathf.Abs(transform.position.x - _startPos.x) > MaxDistance)
            Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        // Destroy upon collision
        Destroy(gameObject);
    }
}
