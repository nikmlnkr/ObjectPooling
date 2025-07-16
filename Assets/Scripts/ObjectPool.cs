using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private readonly Queue<T> pool = new Queue<T>();
    private readonly T prefab;
    private readonly Transform parent;
    private readonly int maxSize;

    public ObjectPool(T prefab, int initialSize = 10, int maxSize = 100, Transform parent = null)
    {
        this.prefab = prefab;
        this.maxSize = maxSize;
        this.parent = parent;

        // Pre-populate the pool
        for (int i = 0; i < initialSize; i++)
        {
            T obj = CreateNewObject();
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public T Get()
    {
        T obj;
        
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = CreateNewObject();
        }

        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Return(T obj)
    {
        if (obj == null) return;

        obj.gameObject.SetActive(false);

        // Only return to pool if we haven't exceeded max size
        if (pool.Count < maxSize)
        {
            pool.Enqueue(obj);
        }
        else
        {
            // Destroy excess objects
            Object.Destroy(obj.gameObject);
        }
    }

    private T CreateNewObject()
    {
        GameObject go = Object.Instantiate(prefab.gameObject, parent);
        return go.GetComponent<T>();
    }

    public int ActiveObjects => prefab ? prefab.transform.childCount - pool.Count : 0;
    public int PooledObjects => pool.Count;
} 