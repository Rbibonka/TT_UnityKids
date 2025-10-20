using System.Collections.Generic;
using UnityEngine;

public class BaseObjectPool<T> where T : PoolableObject
{
    private Queue<T> poolableAvaliableObjects;

    private T poolableObject;
    private Transform objectsParent;

    public BaseObjectPool(T prefab, Transform objectsParent = null)
    {
        this.objectsParent = objectsParent;

        poolableObject = prefab;
        poolableAvaliableObjects = new Queue<T>();
    }

    public void SetToPool(T poolableObject)
    {
        poolableObject.transform.SetParent(objectsParent);

        poolableAvaliableObjects.Enqueue(poolableObject);
    }

    public T GetFromPool()
    {
        if (poolableAvaliableObjects.Count > 0)
        {
            return poolableAvaliableObjects.Dequeue();
        }

        CreateObjects(1);

        return poolableAvaliableObjects.Dequeue();
    }

    private void CreateObjects(int createdObjectCount)
    {
        for (int i = 0; i < createdObjectCount; i++)
        {
            var obj = GameObject.Instantiate(poolableObject, Vector3.zero, Quaternion.identity);

            if (objectsParent != null)
            {
                obj.transform.SetParent(objectsParent);
            }

            poolableAvaliableObjects.Enqueue(obj);
        }
    }
}