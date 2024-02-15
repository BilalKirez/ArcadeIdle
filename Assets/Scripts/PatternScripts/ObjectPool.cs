using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;

    private List<List<GameObject>> poolList;

    void Start()
    {
        InitializePools();
    }

    void InitializePools()
    {
        poolList = new List<List<GameObject>>();

        foreach (Pool pool in pools)
        {
            List<GameObject> objectPool = new List<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, this.gameObject.transform);
                obj.SetActive(false);
                objectPool.Add(obj);
            }

            poolList.Add(objectPool);
        }
    }

    public GameObject GetObjectFromPool(GameObject prefab)
    {
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i].prefab == prefab)
            {
                List<GameObject> objectPool = poolList[i];

                for (int j = 0; j < objectPool.Count; j++)
                {
                    if (!objectPool[j].activeInHierarchy)
                    {
                        objectPool[j].SetActive(true);
                        return objectPool[j];
                    }
                }

                // If all objects in the pool are active, create a new one and add it to the pool
                GameObject newObj = Instantiate(prefab);
                objectPool.Add(newObj);
                return newObj;
            }
        }

        Debug.LogWarning("Pool for prefab " + prefab.name + " not found!");
        return null;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.transform.parent = this.transform;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(false);
    }
}
