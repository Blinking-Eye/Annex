using System;
using UnityEngine;
using System.Collections;

public class LifeAndDeathController : MonoBehaviour {

    /* Health */
    [SerializeField] private float StartingHealth = 100.0f;
    [SerializeField] private LayerMask DefineDamage; // Define what does damage

    private Collider2D[] _colliders;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;

    // Use this for initialization
    void Start()
    {
        CurrentHealth = StartingHealth;
        _colliders = GetComponents<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // If health goes to zero, deactivate and respawn after 5 seconds
        if (CurrentHealth <= 0.0f)
        {
            StartCoroutine("DeathAndRespawn");
        }
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        // Use Damage layer
        if (collision2D.gameObject.tag == "PlayerLemon" || collision2D.gameObject.tag == "EnemyLemon")
        {
            CurrentHealth -= collision2D.gameObject.GetComponent<ProjectileController>().Damage;
        }
    }

    private IEnumerator DeathAndRespawn()
    {
        foreach (Collider2D c in _colliders)
            c.enabled = false;
        _spriteRenderer.enabled = false;
        _rb.isKinematic = true;

        if (CurrentHealth <= 0.0f)
        {
            CurrentHealth = StartingHealth;
            yield return new WaitForSeconds(5); // Wait for 5 seconds
        }
        else
            yield break;

        gameObject.transform.position = Vector3.zero; // Respawn point
        foreach (Collider2D c in _colliders)
            c.enabled = true;
        _spriteRenderer.enabled = true;
        _rb.isKinematic = false;
    }

    /* Getters and Setters */

    // Health
    // TODO: defense(?)
    public float CurrentHealth { get; set; }
}
