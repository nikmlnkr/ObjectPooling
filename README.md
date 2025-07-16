# Object Pooling System for Unity

This is a complete object pooling implementation designed for Unity projects, specifically demonstrated with bullet spawning.

## 📁 Files Overview

- **`ObjectPool.cs`** - Generic object pool class that can pool any GameObject
- **`Bullet.cs`** - Bullet behavior script with automatic pool return
- **`BulletSpawner.cs`** - Manages bullet pool and provides spawning methods
- **`ObjectPoolDemo.cs`** - Demonstration script with user controls

## 🚀 Quick Setup

### 1. Create Bullet Prefab
1. Create an empty GameObject in your scene
2. Add the bullet sprite (drag `Assets/Sprites/bullet.png` to the GameObject)
3. Add the `Bullet.cs` script to this GameObject
4. Add a `Collider2D` component (set as Trigger for collision detection)
5. Save as a prefab in your Assets folder

### 2. Setup Scene
1. Create an empty GameObject named "BulletSpawner"
2. Add the `BulletSpawner.cs` script to it
3. Assign your bullet prefab to the "Bullet Prefab" field
4. Create another empty GameObject named "DemoController"
5. Add the `ObjectPoolDemo.cs` script to it
6. Assign the BulletSpawner to the "Bullet Spawner" field

### 3. Configure Settings
- **Initial Pool Size**: Number of bullets pre-created (default: 20)
- **Max Pool Size**: Maximum bullets in pool (default: 50)
- **Fire Rate**: Time between shots (default: 0.1s)
- **Bullet Speed**: How fast bullets move (default: 10)
- **Bullet Lifetime**: How long bullets live (default: 5s)

## 🎮 Controls

- **Left Mouse Button**: Fire bullet towards mouse cursor
- **Space**: Fire bullet upward
- **B**: Fire burst shot (5 bullets spread)
- **F**: Toggle auto-fire mode

*Note: This system uses Unity's new Input System for modern input handling.*

## 🔧 How It Works

### Object Pool Pattern
The `ObjectPool<T>` class implements the object pool pattern:
- **Pre-allocation**: Creates objects upfront to avoid runtime allocation
- **Reuse**: Inactive objects are reused instead of destroyed
- **Memory Management**: Limits pool size to prevent memory leaks

### Bullet Lifecycle
1. **Get from Pool**: `bulletPool.Get()` retrieves an available bullet
2. **Initialize**: Bullet is positioned and given velocity
3. **Active**: Bullet moves and handles collisions/lifetime
4. **Return**: Bullet automatically returns to pool when done

### Performance Benefits
- **No Garbage Collection**: Reusing objects reduces GC pressure
- **Consistent Performance**: No frame drops from instantiation/destruction
- **Memory Efficient**: Fixed memory usage regardless of bullet count

## 📊 Debug Information

The system displays real-time pool statistics:
- **Pooled Bullets**: Available bullets in pool
- **Active Bullets**: Currently flying bullets

## 🛠 Customization

### Using with Other Objects
The `ObjectPool<T>` is generic and works with any Component:

```csharp
// Example: Enemy pool
ObjectPool<Enemy> enemyPool = new ObjectPool<Enemy>(enemyPrefab, 10, 50);
Enemy enemy = enemyPool.Get();
// ... use enemy ...
enemyPool.Return(enemy);
```

### Custom Bullet Behavior
Modify `Bullet.cs` to add:
- Damage system
- Particle effects
- Sound effects
- Different bullet types

### Advanced Spawning
The `BulletSpawner` supports:
- **Rate limiting**: `TryFireBullet()` respects fire rate
- **Burst fire**: Multiple bullets with spread
- **Direction control**: Fire in any direction

## 🎯 Best Practices

1. **Pool Sizing**: Start with small pools and increase if needed
2. **Reset State**: Always reset object state when returning to pool
3. **Null Checks**: Handle missing pool references gracefully
4. **Performance**: Use pools for frequently created/destroyed objects

## 🐛 Troubleshooting

**Bullets not appearing?**
- Check if bullet prefab is assigned
- Ensure bullet has sprite renderer
- Verify spawn point position

**Performance issues?**
- Reduce initial pool size
- Increase fire rate delay
- Limit max active bullets

**Bullets not returning to pool?**
- Check bullet lifetime settings
- Verify OnBecameInvisible() is called
- Add boundary colliders for return triggers

**Input System errors?**
- Ensure you're using Unity's new Input System package
- If you get "InvalidOperationException" about Input handling, the project is set to use the new Input System
- The demo scripts are designed for the new Input System (not legacy Input Manager)

## 📈 Performance Comparison

Without Object Pooling:
```csharp
// Creates garbage every frame
GameObject bullet = Instantiate(bulletPrefab);
Destroy(bullet, 5f);
```

With Object Pooling:
```csharp
// Reuses existing objects
Bullet bullet = bulletPool.Get();
// ... bullet automatically returns when done
```

This can improve performance by 5-10x in scenarios with many objects!

---

*Happy pooling! 🎯* 
