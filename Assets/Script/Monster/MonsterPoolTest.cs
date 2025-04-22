using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterPoolTest
{
    private List<GameObject> monsterObjPool = new List<GameObject>();
    private List<int> availableLevels = new List<int>();
    private Transform parentTransform;
    private int poolSize;
    private static Dictionary<string, int> monsterLevels = new Dictionary<string, int>();
    private Dictionary<GameObject, (int hp, int damage)> monsterStats = new Dictionary<GameObject, (int, int)>();

    public static Dictionary<string, int> MonsterLevels { get => monsterLevels; set => monsterLevels = value; }

    public MonsterPoolTest(int poolSize, Transform parent, List<MonsterData> monsterTypes)
    {
        this.parentTransform = parent;
        this.poolSize = poolSize;

        InitializeMonsterLevels(monsterTypes); //난이도 설정        
    }

    //몬스터 타입 개수 기반 난이도 설정
    private void InitializeMonsterLevels(List<MonsterData> monsterTypes)
    {        
        for (int i = 1; i <= monsterTypes.Count; i++) //몬스터 타입 개수만큼 난이도 확장
        {
            availableLevels.Add(i);
        }

        foreach (MonsterData data in monsterTypes)
        {
            foreach (var availableLevel in availableLevels)
            {
                MonsterLevels[data.monsterName] = availableLevel; //몬스터에 난이도 설정
                availableLevels.Remove(availableLevel);
                break;
            }
            InitializePool(MonsterLevels[data.monsterName], data.monsterPrefab);
        }
        Debug.Log("몬스터 타입 기반 난이도 설정 완료!");
    }

    //몬스터 생성 및 난이도 기반 스탯 설정
    private void InitializePool(int level, GameObject prefab)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject monster = Object.Instantiate(prefab, parentTransform);
            monster.SetActive(false);

            monsterStats[monster] = (level * 10, level * 2); //HP 및 데미지 설정

            monsterObjPool.Add(monster);
        }
    }

    //몬스터 가져오기
    public GameObject Spawn(Vector3 position)
    {        
        int currentStageLevel = GameManager.Instance.stageLevel; // 현재 스테이지 레벨 가져오기
        foreach (var monster in monsterObjPool)
        {
            if (monsterStats.TryGetValue(monster, out var stats))
            {
                Monster monsterScript = monster.GetComponent<Monster>();
                    monsterScript.SetStats(stats.hp, stats.damage);
                if (stats.hp / 10 == currentStageLevel) // 💡 난이도 필터링 → 해당 난이도에 맞는 몬스터만 선택
                {
                    monster.transform.position = position;
                    monster.SetActive(true);
                    monsterObjPool.Remove(monster);
                    return monster; // 조건에 맞는 몬스터 반환
                }
            }
        }

        Debug.LogWarning($"현재 난이도({currentStageLevel})에 맞는 몬스터가 없습니다!");
        return null; // 적절한 몬스터가 없으면 null 반환

        //if (monsterObjPool.Count >= 0)
        //{
        //    GameObject monster = monsterObjPool[monsterObjPool.Count - 1];
        //    monsterObjPool.RemoveAt(monsterObjPool.Count - 1);

        //    if (monsterStats.TryGetValue(monster, out var stats))
        //    {
        //        if (monsterScript != null)
        //        {
        //        }
        //    }
        //    return monster;
        //}
        //return null;
    }

    //몬스터 반환
    public void Return(GameObject monster)
    {
        if (monster != null)
        {
            monster.SetActive(false);
            monsterObjPool.Add(monster);
        }
        else
        {
            Debug.LogError("반환하려는 몬스터가 null입니다.");
        }
    }
}

