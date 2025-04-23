using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("최소 거리"), Tooltip("몬스터 생성 플레이어 기준 최소거리를 설정할 수 있다."), Range(5, 15)]
    [SerializeField] private float minDis = 10.0f;
    [Header("최대 거리"), Tooltip("몬스터 생성 플레이어 기준 최대거리를 설정할 수 있다."), Range(6, 25)]
    [SerializeField] private float maxDis = 20.0f;
    [Header("소환할 몬스터 데이터"), Tooltip("추후에 리소스폴더에서 가져오면 편하겠지만 지금은 생각X")]
    public List<MonsterData> monsterTypes = new List<MonsterData>(); //몬스터 데이터 리스트
    [Header("몬스터 풀 크기"), Tooltip("MonsterPool에서 크기도 바꿀 수 있긴함 초기 설정은 모두 동일하게 설정 최소치 1")]
    public int poolSize = 100; //풀 크기

    //몬스터 풀링시스템을 담당하는 딕셔너리
    private Dictionary<string, MonsterPool> monsterPools = new Dictionary<string, MonsterPool>();
    //현재 활성화된 몬스터 리스트
    private List<GameObject> activeMonsters = new List<GameObject>();

    //몬스터 스폰 간격 캐싱
    [SerializeField] private WaitForSeconds spawnInterval = new WaitForSeconds(0.3f);

    private void OnValidate()
    {
        if (maxDis < minDis)
        {
            maxDis = minDis;
        }
        if (poolSize < 1)
        {
            poolSize = 1;
        }
    }

    void Start()
    {
        InitializePools(); //몬스터 풀 초기화
        StartCoroutine(SpawnMonsterCor()); // 몬스터 스폰 시작
    }

    //몬스터 풀 초기화 (각 타입별 풀 생성)
    private void InitializePools()
    {
        MonsterPool pool = new MonsterPool(poolSize, transform, monsterTypes);
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
    //플레이어 주변 반경을 기준으로 랜덤한 위치에 몬스터 리젠
    private Vector3 CreateAroundPlayer()
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        float distance = Random.Range(minDis, maxDis);
        return GameManager.Instance.player.transform.position
            + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * distance;
    }
    //몬스터 스폰 루틴 (GameManager 기반)
    private IEnumerator SpawnMonsterCor()
    {
        while (GameManager.Instance.isGameStert)
        {
            int stageLevel = GameManager.Instance.stageLevel;
            yield return spawnInterval;
            int spawnCount = Random.Range(stageLevel * 1, stageLevel * 2);
            ActivateMonsters(spawnCount); //현재 레벨 기반 몬스터 활성화            
        }
    }
    //이전 몬스터 비활성화 및 반환
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
