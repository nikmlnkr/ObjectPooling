using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectPoolDemo : MonoBehaviour
{
    [Header("Demo Settings")]
    public BulletSpawner bulletSpawner;
    public Camera mainCamera;
    
    [Header("Controls")]
    public Key fireKey = Key.Space;
    public Key burstFireKey = Key.B;
    public Key autoFireKey = Key.F;
    
    [Header("Auto Fire Settings")]
    public bool autoFire = false;
    public float autoFireRate = 0.05f;
    
    private bool isAutoFiring = false;
    private float nextAutoFireTime;
    
    private void Start()
    {
        // Get camera if not assigned
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                mainCamera = FindObjectOfType<Camera>();
            }
        }
        
        // Get bullet spawner if not assigned
        if (bulletSpawner == null)
        {
            bulletSpawner = FindObjectOfType<BulletSpawner>();
        }
        
        if (bulletSpawner == null)
        {
            Debug.LogError("No BulletSpawner found! Please assign one or add a BulletSpawner to the scene.");
        }
        
        Debug.Log("=== Object Pool Demo Controls ===");
        Debug.Log($"Left Mouse Button: Fire bullet towards mouse");
        Debug.Log($"{fireKey}: Fire bullet upward");
        Debug.Log($"{burstFireKey}: Fire burst shot");
        Debug.Log($"{autoFireKey}: Toggle auto fire");
        Debug.Log("==================================");
    }
    
    private void Update()
    {
        HandleInput();
        HandleAutoFire();
    }
    
    private void HandleInput()
    {
        if (bulletSpawner == null) return;
        
        // Check if input devices are available
        var mouse = Mouse.current;
        var keyboard = Keyboard.current;
        
        if (mouse == null || keyboard == null) return;
        
        // Mouse shooting - fire towards mouse position
        if (mouse.leftButton.wasPressedThisFrame)
        {
            FireTowardsMouse();
        }
        
        // Keyboard shooting - fire upward
        if (keyboard[fireKey].wasPressedThisFrame)
        {
            bulletSpawner.TryFireBullet(Vector2.up);
        }
        
        // Burst fire
        if (keyboard[burstFireKey].wasPressedThisFrame)
        {
            bulletSpawner.FireBurst(Vector2.up, 5, 30f);
        }
        
        // Toggle auto fire
        if (keyboard[autoFireKey].wasPressedThisFrame)
        {
            isAutoFiring = !isAutoFiring;
            Debug.Log($"Auto fire: {(isAutoFiring ? "ON" : "OFF")}");
        }
    }
    
    private void HandleAutoFire()
    {
        if (!isAutoFiring || bulletSpawner == null) return;
        
        if (Time.time >= nextAutoFireTime)
        {
            // Fire in a circular pattern
            float angle = Time.time * 90f; // Rotate 90 degrees per second
            Vector2 direction = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );
            
            bulletSpawner.FireBullet(direction);
            nextAutoFireTime = Time.time + autoFireRate;
        }
    }
    
    private void FireTowardsMouse()
    {
        if (mainCamera == null || bulletSpawner == null) return;
        
        var mouse = Mouse.current;
        if (mouse == null) return;
        
        // Get mouse position in world space
        Vector2 mousePos = mouse.position.ReadValue();
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCamera.nearClipPlane));
        
        // Calculate direction from spawner to mouse
        Vector2 direction = (worldPos - bulletSpawner.transform.position).normalized;
        
        bulletSpawner.TryFireBullet(direction);
    }
    
    // Display controls on screen
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, Screen.height - 150, 300, 140));
        GUILayout.Box("=== CONTROLS ===");
        GUILayout.Label("Left Click: Fire towards mouse");
        GUILayout.Label($"{fireKey}: Fire upward");
        GUILayout.Label($"{burstFireKey}: Burst fire (5 bullets)");
        GUILayout.Label($"{autoFireKey}: Auto fire: " + (isAutoFiring ? "ON" : "OFF"));
        GUILayout.EndArea();
    }
} 