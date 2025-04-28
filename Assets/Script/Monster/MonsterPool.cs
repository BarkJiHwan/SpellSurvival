using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool
{
    private List<GameObject> monsterObjPool = new List<GameObject>();//���� Ǯ
    private Transform parentTransform; //�θ�ü(������)
    private int poolSize; //Ǯ������
    private Dictionary<string, int> monsterLevels = new Dictionary<string, int>();

    public MonsterPool(int poolSize, Transform parent, List<MonsterData> monsterTypes)
    {
        this.parentTransform = parent;
        this.poolSize = poolSize;

        InitializeMonsterLevels(monsterTypes); //���̵� ����        
    }
    private void InitializeMonsterLevels(List<MonsterData> monsterTypes)
    {
        List<int> availableLevels = new List<int>();
        //���̵� ����Ʈ �ʱ�ȭ �� ����
        availableLevels = new List<int>();
        for (int i = 1; i <= monsterTypes.Count; i++)
        {
            availableLevels.Add(i);
        }
        ShuffleList(availableLevels); //���̵� ����Ʈ ����
        
        //���� Ÿ�Կ� ���̵� �Ҵ�
        for (int i = 0; i < monsterTypes.Count; i++)
        {
            monsterLevels[monsterTypes[i].monsterName] = availableLevels[i];
            InitializePool(monsterLevels[monsterTypes[i].monsterName], monsterTypes[i]);
        }
    }
    //����Ʈ ���� �Լ�
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

    //���� ���� �� ���̵� ��� ���� ����(��ũ���ͺ� ������Ʈ�� ������ �ϱ� �����Ƽ�....)
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

    //���� ��������
    public GameObject Spawn(Vector3 position)
    {
        int currentStageLevel = GameManager.Instance.stageLevel; //���� �������� ���� ��������
        foreach (var monster in monsterObjPool)
        {
            Monster monsterScript = monster.GetComponent<Monster>();
            if (monsterScript.monsterData.level == currentStageLevel)
            {
                monster.transform.position = position;
                monster.SetActive(true);
                monsterObjPool.Remove(monster);
                return monster; //���ǿ� �´� ���� ��ȯ
            }
        }
        return null; //������ ���Ͱ� ������ null ��ȯ
    }

    //���� ��ȯ
    public void Return(GameObject monster)
    {
        if (monster != null)
        {
            monster.SetActive(false);
            monsterObjPool.Add(monster);
        }
        else
        {
            Debug.Log("���Ͱ� ������ ���θ���� ��� ��Ƚ��ϴ�.");
            GameObject newMonster = Object.Instantiate(monster, parentTransform);
            monster.SetActive(false);
            monsterObjPool.Add(monster);
        }
    }
}
