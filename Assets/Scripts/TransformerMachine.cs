using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformerMachine : MonoBehaviour
{
    public float transformTime;
    public int inputCapacity = 20;
    public int outputCapacity = 20;
    public List<GameObject> inputObjects = new List<GameObject>();
    public List<GameObject> outputObjects = new List<GameObject>();
    public Transform inputTransform;
    public Transform outputTransform;
    public Animator animator;
    public GameObject outputObject;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        InvokeRepeating("ProcessItems", 0f, transformTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (inputObjects.Count > 0 && outputObjects.Count < outputCapacity)
        {
            animator.enabled = true;
        }
    }
    public void ProcessItems()
    {
        if (!animator.enabled)
        {
            return;
        }
        AssetTransformation();
        CheckMachineForTransformation();
    }
    public void AssetTransformation()
    {
        var transformedAsset = inputObjects[inputObjects.Count - 1];
        inputObjects.Remove(transformedAsset);
        ObjectPool.Instance.ReturnObjectToPool(transformedAsset);
        var lastProduct = ObjectPool.Instance.GetObjectFromPool(outputObject);
        lastProduct.transform.position = outputTransform.position + CalculatePosition(outputObjects.Count);
        outputObjects.Add(lastProduct);
    }
    public void CheckMachineForTransformation()
    {
        if (outputObjects.Count >= outputCapacity || inputObjects.Count <= 0)
        {
            animator.enabled = false;
        }
    }

    private Vector3 CalculatePosition(int productCount)
    {
        var calculatedVector = new Vector3(0, (productCount / 5) * 0.2f, (productCount % 5) * 0.2f - 0.4f);
        return calculatedVector;
    }
    public bool EnoughSpaceForStorage()
    {
        if (inputObjects.Count < inputCapacity)
        {
            return true;
        }
        return false;
    }
    public void DroppedItem(GameObject droppedItem)
    {
        if (droppedItem == null)
        {
            return;
        }
        inputObjects.Add(droppedItem);
        droppedItem.transform.parent = inputTransform;
        droppedItem.transform.position = inputTransform.position + CalculatePosition(inputObjects.Count - 1);
        droppedItem.transform.rotation = Quaternion.identity;
    }
    public GameObject CollectedItem()
    {
        var collectedItem = outputObjects[outputObjects.Count - 1];
        outputObjects.Remove(collectedItem);
        return collectedItem;
    }
}
