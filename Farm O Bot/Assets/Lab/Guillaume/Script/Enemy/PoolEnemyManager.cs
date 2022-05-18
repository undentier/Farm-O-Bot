using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolEnemyManager : MonoBehaviour
{
    public static PoolEnemyManager instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public Transform parentStoring;
        public GameObject prefab;
        public int sizeOfPool;
    }

    public List<Pool> pools;
    public Dictionary<string, List<GameObject>> poolDictionary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Care there is multiple PoolEnemyManager in the scene");
        }
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, List<GameObject>>();

        foreach (Pool pool in pools)
        {
            List<GameObject> objectPool = new List<GameObject>();

            for (int i = 0; i < pool.sizeOfPool; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                objectPool.Add(obj);
                obj.transform.SetParent(pool.parentStoring);
                obj.SetActive(false);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 _position, Quaternion _rotation)
    {
        if (poolDictionary.ContainsKey(tag) == false)
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't excist");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag][0];
        poolDictionary[tag].RemoveAt(0);

        objectToSpawn.transform.position = _position;
        objectToSpawn.transform.rotation = _rotation;
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    public void AddObjectToPool(string tag, GameObject objToAdd)
    {
        if (poolDictionary.ContainsKey(tag) == false)
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't excist");
            return;
        }

        poolDictionary[tag].Add(objToAdd);
        objToAdd.SetActive(false);
    }
}
