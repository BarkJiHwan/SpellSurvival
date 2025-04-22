using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolTest
{
    private List<GameObject> monsterObjPool = new List<GameObject>();
    private GameObject prefab;
    private Transform parentTransform;
    private int poolSize;
    private static Dictionary<string, int> monsterLevels = new Dictionary<string, int>();
    private Dictionary<GameObject, (int hp, int damage)> monsterStats = new Dictionary<GameObject, (int, int)>();

    public static Dictionary<string, int> MonsterLevels { get => monsterLevels; set => monsterLevels = value; }

    public MonsterPoolTest(GameObject prefab, int poolSize, Transform parent, List<MonsterData> monsterTypes)
    {
        this.prefab = prefab;
        this.parentTransform = parent;
        this.poolSize = poolSize;

        InitializeMonsterLevels(monsterTypes); //난이도 설정        
    }

    //몬스터 타입 개수 기반 난이도 설정
    private void InitializeMonsterLevels(List<MonsterData> monsterTypes)
    {
        List<int> availableLevels = new List<int>();
        
        for (int i = 1; i <= monsterTypes.Count; i++) //몬스터 타입 개수만큼 난이도 확장
        {
            availableLevels.Add(i);
        }
        foreach (var availableLevel in availableLevels)
        {
            Debug.Log(availableLevel + "각 숫자는 몇임?");
        }
        int level = 0;
        foreach (MonsterData data in monsterTypes)
        {
            if (availableLevels.Count == 0)
            {
                break;
            }
            int randomIndex = Random.Range(0, availableLevels.Count);
            int selectedLevel = availableLevels[randomIndex];

            MonsterLevels[data.monsterName] = selectedLevel; //몬스터에 난이도 설정
            level = MonsterLevels[data.monsterName];
            availableLevels.RemoveAt(randomIndex); //중복 방지
        }
        Debug.Log(level + "레벨");
        InitializePool(level);
        Debug.Log("몬스터 타입 기반 난이도 설정 완료!");
    }

    //몬스터 생성 및 난이도 기반 스탯 설정
    private void InitializePool(int level)
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
        if (monsterObjPool.Count > 0)
        {
            GameObject monster = monsterObjPool[monsterObjPool.Count - 1];
            monsterObjPool.RemoveAt(monsterObjPool.Count - 1);
            monster.transform.position = position;
            monster.SetActive(true);

            if (monsterStats.TryGetValue(monster, out var stats))
            {
                Monster monsterScript = monster.GetComponent<Monster>();
                if (monsterScript != null)
                {
                    monsterScript.SetStats(stats.hp, stats.damage);
                }
            }

            return monster;
        }
        else
        {
            Debug.LogWarning("MonsterPool이 비어 있음! 새로운 몬스터 생성 필요.");
            return null;
        }
    }

    //몬스터 반환
    public void Return(GameObject monster)
    {
        if (monster != null)
        {
            monster.SetActive(false);
            monsterObjPool.Add(monster); //리스트 끝에 추가(추후 재활용 가능할까 싶어서 일단저장)
        }
        else
        {
            Debug.LogError("반환하려는 몬스터가 null입니다.");
        }
    }
}

