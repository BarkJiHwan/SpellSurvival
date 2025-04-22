using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class MonsterSpawnerTest : MonoBehaviour
{
    float minDis = 10.0f;
    float maxDis = 20.0f;
    public List<MonsterData> monsterTypes = new List<MonsterData>(); //몬스터 데이터 리스트
    private Dictionary<string, MonsterPoolTest> monsterPools = new Dictionary<string, MonsterPoolTest>(); //몬스터 풀링 시스템
    private List<GameObject> activeMonsters = new List<GameObject>(); //현재 활성화된 몬스터 리스트

    public int poolSize = 100; //풀 크기
    private WaitForSeconds spawnInterval = new WaitForSeconds(1); //스폰 간격

    void Start()
    {
        InitializePools(); //몬스터 풀 초기화
        StartCoroutine(SpawnMonsterCor()); // 몬스터 스폰 시작
    }

    //몬스터 풀 초기화 (각 타입별 풀 생성)
    private void InitializePools()
    {
        MonsterPoolTest pool = new MonsterPoolTest(poolSize, transform, monsterTypes);
        foreach (MonsterData data in monsterTypes)
        {
            if (!monsterPools.ContainsKey(data.monsterName))
            {
                monsterPools.Add(data.monsterName, pool);
            }
        }
    }
    void SpawnMonster(string monsterType)
    {
        if (monsterPools.ContainsKey(monsterType))
        {
            Vector3 spawnPos = CreateAroundPlayer();
            monsterPools[monsterType].Spawn(spawnPos);
        }
    }
    //현재 스테이지 레벨에 맞는 몬스터 활성화
    private void ActivateMonsters(int count)
    {
        for (int i = 0; i < count; i++)
        {
            string monsterType = monsterTypes[Random.Range(0, monsterTypes.Count)].monsterName;
            SpawnMonster(monsterType);
        }        
    }

    private Vector3 CreateAroundPlayer()
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        float distance = Random.Range(minDis, maxDis);
        return GameManager.Instance.player.transform.position
            + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * distance;
    }

    //이전 몬스터 비활성화 및 반환
    private void RemovePreviousMonsters()
    {
        foreach (GameObject monster in activeMonsters)
        {
            if (monsterPools.TryGetValue(monster.name, out MonsterPoolTest pool))
            {
                pool.Return(monster);
            }
        }

        activeMonsters.Clear(); //리스트 초기화
    }

    //몬스터 스폰 루틴 (GameManager 기반)
    private IEnumerator SpawnMonsterCor()
    {
        while (true)
        {
            yield return spawnInterval;

            //RemovePreviousMonsters(); // 이전 몬스터 정리
            int spawnCount = Random.Range(5, 20);
            ActivateMonsters(spawnCount); //현재 레벨 기반 몬스터 활성화

            Debug.Log($"현재 스테이지 레벨: {GameManager.Instance.stageLevel} - 새로운 몬스터 활성화 완료!");
        }
    }
    public void ReturnMonster(GameObject monster)
    {
        monster.SetActive(false);
        Monster monsterComponent = monster.GetComponent<Monster>();
        if (monsterComponent != null && monsterComponent.monsterData != null)
        {
            string monsterType = monsterComponent.monsterData.monsterName;
            if (monsterPools.ContainsKey(monsterType))
            {
                monsterPools[monsterType].Return(monster);
            }
        }
    }
}