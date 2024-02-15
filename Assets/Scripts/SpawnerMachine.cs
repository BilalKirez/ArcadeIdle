using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMachine : MonoBehaviour
{
    public float spawnTime;
    public int storageCapacity = 20;
    public Animator animator;
    public GameObject spawnObject;

    public GameObject spawnPoint;
    public List<GameObject> spawnObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        InvokeRepeating("CreateSpawnObjects", 0f, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnObjects.Count < storageCapacity)
        {
            animator.enabled = true;
        }
    }

    public void CreateSpawnObjects()
    {
        if (!animator.enabled)
        {
            return;
        }
        SpawnObject();
        CheckStorageForCreate();
    }
    public void CheckStorageForCreate()
    {
        if (spawnObjects.Count >= storageCapacity)
        {
            animator.enabled = false;
        }
    }

    private void SpawnObject()
    {
        var obj = ObjectPool.Instance.GetObjectFromPool(spawnObject);
        obj.transform.position = spawnPoint.transform.position + CalculatePosition(spawnObjects.Count);
        spawnObjects.Add(obj);
    }
    private Vector3 CalculatePosition(int productCount)
    {
        var calculatedVector = new Vector3(0, (productCount / 5) * 0.2f, (productCount % 5) * 0.2f - 0.4f);
        return calculatedVector;
    }

    public GameObject CollectedItem()
    {
        var collectedItem = spawnObjects[spawnObjects.Count - 1];
        spawnObjects.Remove(collectedItem);
        return collectedItem;
    }
}
