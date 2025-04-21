using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    float minDis = 10.0f;
    float maxDis = 20.0f; 
    public List<MonsterData> monsterTypes;
    private Dictionary<string, MonsterPool> monsterPools = new Dictionary<string, MonsterPool>();    
    public int poolSize = 100;

    WaitForSeconds seconds = new WaitForSeconds(1);
    
    public float timer;
    void Start()
    {
        foreach(MonsterData data in monsterTypes)
        {
            MonsterPool pool = new MonsterPool(data.monsterPrefab, poolSize, transform);
            monsterPools.Add(data.monsterName, pool);
        }
        StartCoroutine(SpawnMonsterCor());
    }
    void SpawnMonster(string monsterType)
    {
        if(monsterPools.ContainsKey(monsterType))
        {
            Vector3 spawnPos = CreateAroundPlayer();
            monsterPools[monsterType].Spawn(spawnPos);
        }
    }

    private Vector3 CreateAroundPlayer()
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        float distance = Random.Range(minDis, maxDis);
        return GameManager.Instance.player.transform.position
            + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * distance;
    }
    public void ReturnMonster(GameObject monster)
    {
        monster.SetActive(false);
        Monster monsterComponent = monster.GetComponent<Monster>();
        if(monsterComponent != null && monsterComponent.monsterData != null)
        {
            string monsterType = monsterComponent.monsterData.monsterName;
            if(monsterPools.ContainsKey(monsterType))
            {
                monsterPools[monsterType].Reture(monster);
            }
        }
    }

    IEnumerator SpawnMonsterCor()
    {
        while (true)
        {
            yield return seconds;
            int spawnCount = Random.Range(25, 50);
            SpawnMonsterBatch(spawnCount);            
        }
    }
    private void SpawnMonsterBatch(int count)
    {
        for (int i = 0; i < count; i++)
        {
            string monsterType = monsterTypes[Random.Range(0, monsterTypes.Count)].monsterName;
            SpawnMonster(monsterType);
        }
    }
}
