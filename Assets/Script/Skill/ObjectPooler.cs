using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public int maxSize;
    }

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        //풀 만들기
        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {                
                GameObject obj = Instantiate(pool.prefab);                
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public void CreatePool(string tag, GameObject prefab, int defaultSize, int maxSize)
    {
        if (poolDictionary.ContainsKey(tag))
            return;

        Pool newPool = new Pool
        {
            tag = tag,
            prefab = prefab,
            size = defaultSize,
            maxSize = maxSize
        };

        pools.Add(newPool);

        Queue<GameObject> objectPool = new Queue<GameObject>();

        for (int i = 0; i < defaultSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        poolDictionary.Add(tag, objectPool);
    }

    /// <summary>
    /// 풀에서 스폰 요청 (없으면 자동 생성)
    /// </summary>
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            // 자동 생성 시도
            Debug.LogWarning($"No pool found for {tag}. Trying to auto-create pool.");

            ActiveSkillData skillData = SkillManager.Instance.GetSkillData(tag);
            if (skillData != null)
            {
                CreatePool(tag, skillData.skillPrefab, 5, 20);
            }
            else
            {
                Debug.LogError($"Cannot auto-create pool: SkillData not found for {tag}");
                return null;
            }
        }

        Queue<GameObject> poolQueue = poolDictionary[tag];

        if (poolQueue.Count == 0)
        {
            Pool poolConfig = pools.Find(p => p.tag == tag);
            if (poolConfig != null && poolQueue.Count < poolConfig.maxSize)
            {
                GameObject obj = Instantiate(poolConfig.prefab);
                obj.SetActive(false);
                poolQueue.Enqueue(obj);
            }
            else
            {
                Debug.LogError($"Pool {tag} is empty and cannot expand!");
                return null;
            }
        }

        GameObject objectToSpawn = poolQueue.Dequeue();
        objectToSpawn.SetActive(true);        
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        return objectToSpawn;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        poolDictionary[obj.name.Replace("(Clone)", "").Trim()].Enqueue(obj);
    }

}
