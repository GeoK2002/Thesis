/*This script handles the projectile's movement, ereasure after hitting an enemy and dealing damage*/
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 40;
    public float lifetime = 2f;
    public float knockbackForce = 10f;
    private Rigidbody2D rb;
    private Vector2 direction;
    public PlayerMovement playerMovement;
    public PlayerAttack playerAttack;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = direction * speed;
        }

        Destroy(gameObject, lifetime);
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction; // Store the direction
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (rb != null)
        {
            rb.velocity = direction * speed;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = direction.x < 0;
            }
        }
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Enemy"))
        {
            EnemyHealth enemy = hitInfo.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Vector2 knockbackDirection = (hitInfo.transform.position - transform.position).normalized;
                if (enemy.GetComponent<Rigidbody2D>() != null)
                {
                    enemy.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                }

                Destroy(gameObject);
            }
        }
        if (playerAttack != null)
            {
                playerAttack.ProjectileDestroyed(); 
            }
        
        else if (hitInfo.CompareTag("Collectable"))
        {
            Destroy(gameObject);
            if (playerAttack != null)
            {
                playerAttack.ProjectileDestroyed();
            }
        }
    }
}
