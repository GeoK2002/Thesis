/*This script controlls the enemy so it always follows the player*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private Transform target;
    public float speed;
    private bool isFacingRight = true;
    private Rigidbody2D rb;
    private bool isKnockedBack = false;
    [SerializeField] private float knockbackDuration = 0.2f; 
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float followRange = 5f; 

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isKnockedBack && target != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);

            if (distanceToPlayer <= followRange)
            {
                FollowPlayer();
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void FollowPlayer()
    {
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (target.position.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }
        else if (target.position.x < transform.position.x && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void ApplyKnockback(Vector2 knockbackDirection)
    {
        if (!isKnockedBack)
        {
            isKnockedBack = true;
            rb.velocity = knockbackDirection.normalized * knockbackForce;

            StartCoroutine(ResetKnockback());
        }
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false;
        rb.velocity = Vector2.zero;
    }
}
