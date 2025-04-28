using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool
{
    private List<GameObject> monsterObjPool = new List<GameObject>();//몬스터 풀
    private Transform parentTransform; //부모객체(스포너)
    private int poolSize; //풀사이즈
    private Dictionary<string, int> monsterLevels = new Dictionary<string, int>();

    public MonsterPool(int poolSize, Transform parent, List<MonsterData> monsterTypes)
    {
        this.parentTransform = parent;
        this.poolSize = poolSize;

        InitializeMonsterLevels(monsterTypes); //난이도 설정        
    }
    private void InitializeMonsterLevels(List<MonsterData> monsterTypes)
    {
        List<int> availableLevels = new List<int>();
        //난이도 리스트 초기화 및 셔플
        availableLevels = new List<int>();
        for (int i = 1; i <= monsterTypes.Count; i++)
        {
            availableLevels.Add(i);
        }
        ShuffleList(availableLevels); //난이도 리스트 셔플
        
        //몬스터 타입에 난이도 할당
        for (int i = 0; i < monsterTypes.Count; i++)
        {
            monsterLevels[monsterTypes[i].monsterName] = availableLevels[i];
            InitializePool(monsterLevels[monsterTypes[i].monsterName], monsterTypes[i]);
        }
    }
    //리스트 셔플 함수
    private void ShuffleList<T>(List<T> list)
    {
        int count = list.Count;
        while (count > 1)
        {
            count--;
            int num = Random.Range(0, count + 1);
            T value = list[num];
            list[num] = list[count];
            list[count] = value;
        }
    }

    //몬스터 생성 및 난이도 기반 스탯 설정(스크립터블 오브젝트로 일일히 하기 귀찮아서....)
    private void InitializePool(int level, MonsterData monsterData)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject monster = Object.Instantiate(monsterData.monsterPrefab, parentTransform);
            monsterData.level = level;
            monsterData.hp = level * 10;
            monsterData.damage = (1 + (level * 2) / 2);
            monsterData.speed = (2 + level * 0.25f);
            monsterData.rotationSpeed = monsterData.speed;
            monster.SetActive(false);
            monsterObjPool.Add(monster);
        }
    }

    //몬스터 가져오기
    public GameObject Spawn(Vector3 position)
    {
        int currentStageLevel = GameManager.Instance.stageLevel; //현재 스테이지 레벨 가져오기
        foreach (var monster in monsterObjPool)
        {
            Monster monsterScript = monster.GetComponent<Monster>();
            if (monsterScript.monsterData.level == currentStageLevel)
            {
                monster.transform.position = position;
                monster.SetActive(true);
                monsterObjPool.Remove(monster);
                return monster; //조건에 맞는 몬스터 반환
            }
        }
        return null; //적절한 몬스터가 없으면 null 반환
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
            Debug.Log("몬스터가 터져서 새로만들어 담아 드렸습니다.");
            GameObject newMonster = Object.Instantiate(monster, parentTransform);
            monster.SetActive(false);
            monsterObjPool.Add(monster);
        }
    }
}
