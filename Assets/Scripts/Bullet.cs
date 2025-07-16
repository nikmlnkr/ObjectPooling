using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f;
    public float lifetime = 5f;
    
    private ObjectPool<Bullet> pool;
    private Rigidbody2D rb;
    private Vector2 direction;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0f; // No gravity for bullets
    }
    
    public void Initialize(ObjectPool<Bullet> bulletPool, Vector2 shootDirection, Vector3 startPosition)
    {
        pool = bulletPool;
        direction = shootDirection.normalized;
        transform.position = startPosition;
        
        // Set velocity
        rb.linearVelocity = direction * speed;
        
        // Start the lifetime countdown
        StartCoroutine(LifetimeCountdown());
    }
    
    private IEnumerator LifetimeCountdown()
    {
        yield return new WaitForSeconds(lifetime);
        ReturnToPool();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // You can add collision logic here
        // For now, just return to pool on any collision
        if (other.CompareTag("Enemy") || other.CompareTag("Wall"))
        {
            ReturnToPool();
        }
    }
    
    private void OnBecameInvisible()
    {
        // Return to pool when bullet goes off screen
        ReturnToPool();
    }
    
    public void ReturnToPool()
    {
        // Stop all coroutines
        StopAllCoroutines();
        
        // Reset velocity
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
        
        // Return to pool
        if (pool != null)
        {
            pool.Return(this);
        }
        else
        {
            // Fallback: destroy if no pool reference
            Destroy(gameObject);
        }
    }
    
    private void OnDisable()
    {
        // Clean up when object is disabled
        StopAllCoroutines();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
} 