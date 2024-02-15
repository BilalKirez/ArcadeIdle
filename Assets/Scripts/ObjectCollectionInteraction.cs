using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectCollectionInteraction : MonoBehaviour
{
    public int carryItemCapacity;
    public List<GameObject> itemList = new List<GameObject>();
    public Transform alignTransform;
    public bool collect;
    public bool drop;
    public bool output;
    public bool trash;
    public SpawnerMachine spawnerMachine;
    public TransformerMachine transformerMachine;
    public IEnumerator CollectItems()
    {
        if (collect)
        {
            while (collect)
            {
                if (itemList.Count < carryItemCapacity && spawnerMachine.spawnObjects.Count > 0)
                {
                    var collectedObject = spawnerMachine.CollectedItem();
                    AddItemYourInventory(collectedObject);
                }
                yield return new WaitForSeconds(0.2f);
            }
        }
        if (output)
        {
            while (output)
            {
                if (itemList.Count < carryItemCapacity && transformerMachine.outputObjects.Count > 0)
                {
                    var collectedObject = transformerMachine.CollectedItem();
                    AddItemYourInventory(collectedObject);
                }
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
    public IEnumerator DropItems()
    {
        if (drop)
        {
            while (drop)
            {
                if (itemList.Count > 0 && transformerMachine.EnoughSpaceForStorage())
                {
                    var dropItem = itemList.Where(x => x.CompareTag("spawnObject")).LastOrDefault();
                    itemList.Remove(dropItem);
                    transformerMachine.DroppedItem(dropItem);
                }
                yield return new WaitForSeconds(0.2f);
            }
        }
        if (trash)
        {
            while (trash)
            {
                if (itemList.Count > 0)
                {
                    var droppedItem = itemList[itemList.Count - 1];
                    itemList.Remove(droppedItem);
                    //Destroy(droppedItem);
                    ObjectPool.Instance.ReturnObjectToPool(droppedItem);
                }
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
    public void AddItemYourInventory(GameObject collectedObject)
    {
        collectedObject.transform.parent = this.gameObject.transform;
        collectedObject.transform.position = alignTransform.position + new Vector3(0f, 0.1f * itemList.Count, 0f);
        collectedObject.transform.rotation = Quaternion.identity;
        itemList.Add(collectedObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("collectArea"))
        {
            collect = true;
            StartCoroutine(nameof(CollectItems));
        }
        if (other.CompareTag("outputArea"))
        {
            output = true;
            StartCoroutine(nameof(CollectItems));
        }
        if (other.CompareTag("loadingArea"))
        {
            drop = true;
            StartCoroutine(nameof(DropItems));
        }
        if (other.CompareTag("trash"))
        {
            trash = true;
            StartCoroutine(nameof(DropItems));
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("collectArea"))
        {
            collect = false;
        }
        if (other.CompareTag("outputArea"))
        {
            output = false;
        }
        if (other.CompareTag("loadingArea"))
        {
            drop = false;
        }
        if (other.CompareTag("trash"))
        {
            trash = false;
        }
    }
}
