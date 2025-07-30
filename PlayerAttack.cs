/*Here the calculations for the player attacking with the short range attack and the projectile are made*/

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public PlayerMovement playerMovement;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 50;
    public int maxProjectiles = 5;
    public int currentProjectiles = 5;
    public float knockbackForce = 10f;
    public float knockbackDelay = 0.2f;
    public float attackCooldown = 1.0f;  
    private bool canAttack = true;  
    private float lastAttackTime = 0.0f;
    public Text projectileCounterText;

    void Start()
    {
        currentProjectiles=5;
        UpdateProjectileCounterUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canAttack) 
        {
            Attack();
        }

        if (!canAttack)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                canAttack = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && canAttack && Time.time >= lastAttackTime + attackCooldown)
        {
            Shoot();
            lastAttackTime = Time.time;
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        canAttack = false;
        lastAttackTime = Time.time;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
            StartCoroutine(ApplyKnockbackWithDelay(enemy));
        }
                
        StartCoroutine(EnableFlippingAfterAttack());
    }

    void Shoot()
    {
        if (currentProjectiles ==0)
        {
        Debug.Log("Maximum number of projectiles reached.");
        return;
        }
        GameObject projectileObject = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectileScript = projectileObject.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            Vector2 shootDirection = firePoint.right;

            if (!playerMovement.isFacingRight) 
            {
                shootDirection = -firePoint.right;
            }

            projectileScript.Initialize(shootDirection);
            projectileScript.playerMovement = playerMovement;
        }
        currentProjectiles--;
        UpdateProjectileCounterUI();
    }

    public void ProjectileDestroyed()
    {
        if (currentProjectiles > 0)
        {
            currentProjectiles--;
            UpdateProjectileCounterUI();
        }
    }

    public void UpdateProjectileCounterUI()
    {
        if (projectileCounterText != null)
        {
            projectileCounterText.text = "Number of Projectiles Left: " + currentProjectiles + "/" + maxProjectiles;
        }
    }

    IEnumerator EnableFlippingAfterAttack()
    {
        playerMovement.canFlip = false;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        playerMovement.canFlip = true;
    }

    IEnumerator ApplyKnockbackWithDelay(Collider2D enemy)
    {
        yield return new WaitForSeconds(knockbackDelay);

        Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
        enemy.GetComponent<EnemyFollow>().ApplyKnockback(knockbackDirection * knockbackForce);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
