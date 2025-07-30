/*Player Movement*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed= 8f;
    private float jumpingPower= 16f;
    public bool isFacingRight= true; 
    public HealthManager healthManager;
    public Animator animator;
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float knockbackForce = 10f;
    private bool isKnockedBack = false;
    public bool canFlip = true;

    void Update()
    {

        if(!isKnockedBack)
        {
        horizontal = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        
        if(Input.GetButtonDown("Jump") && IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                animator.SetTrigger("Jump");
            }
            
        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        
        if (canFlip)
            {
                Flip();
            }
        FixedUpdate();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            healthManager.TakeDamage(20);
            ApplyKnockback(collision.transform);
        }
    }
    
    public void FixedUpdate()
    {
        if(!isKnockedBack)
        {

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        }
    }
    
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);
    }
    
    public void Flip()
    {

        if (isFacingRight && horizontal <0f || !isFacingRight && horizontal >0f)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
    }

     private void ApplyKnockback(Transform enemyTransform)
    {
        isKnockedBack = true;
        Vector2 knockbackDirection = (transform.position - enemyTransform.position).normalized;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        StartCoroutine(ResetKnockback());
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false;
    }
    
}
