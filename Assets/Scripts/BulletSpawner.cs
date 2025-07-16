using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [Header("Bullet Pool Settings")]
    public Bullet bulletPrefab;
    public int initialPoolSize = 20;
    public int maxPoolSize = 50;
    
    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public float fireRate = 0.1f; // Time between shots
    
    private ObjectPool<Bullet> bulletPool;
    private float lastFireTime;
    
    private void Start()
    {
        // Validate required components
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab is not assigned!");
            return;
        }
        
        if (spawnPoint == null)
        {
            spawnPoint = transform; // Use this transform as spawn point if none specified
        }
        
        // Create the bullet pool
        GameObject poolParent = new GameObject("Bullet Pool");
        poolParent.transform.SetParent(transform);
        
        bulletPool = new ObjectPool<Bullet>(
            bulletPrefab, 
            initialPoolSize, 
            maxPoolSize, 
            poolParent.transform
        );
        
        Debug.Log($"Bullet pool initialized with {initialPoolSize} bullets");
    }
    
    public bool TryFireBullet(Vector2 direction)
    {
        // Check fire rate
        if (Time.time - lastFireTime < fireRate)
        {
            return false;
        }
        
        return FireBullet(direction);
    }
    
    public bool FireBullet(Vector2 direction)
    {
        if (bulletPool == null || bulletPrefab == null)
        {
            Debug.LogWarning("Bullet pool or prefab is null!");
            return false;
        }
        
        // Get bullet from pool
        Bullet bullet = bulletPool.Get();
        
        if (bullet != null)
        {
            // Initialize the bullet
            bullet.Initialize(bulletPool, direction, spawnPoint.position);
            lastFireTime = Time.time;
            return true;
        }
        
        return false;
    }
    
    public void FireBurst(Vector2 direction, int bulletCount, float spreadAngle = 15f)
    {
        if (bulletCount <= 1)
        {
            FireBullet(direction);
            return;
        }
        
        float angleStep = spreadAngle / (bulletCount - 1);
        float startAngle = -spreadAngle / 2f;
        
        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            Vector2 spreadDirection = RotateVector(direction, currentAngle);
            FireBullet(spreadDirection);
        }
    }
    
    private Vector2 RotateVector(Vector2 vector, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        
        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }
    
    // Debug info
    private void OnGUI()
    {
        if (bulletPool != null && Application.isPlaying)
        {
            GUILayout.BeginArea(new Rect(10, 10, 200, 100));
            GUILayout.Label($"Pooled Bullets: {bulletPool.PooledObjects}");
            GUILayout.Label($"Active Bullets: {bulletPool.ActiveObjects}");
            GUILayout.EndArea();
        }
    }
} 