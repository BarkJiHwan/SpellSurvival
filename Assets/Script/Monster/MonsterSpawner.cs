using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("�ּ� �Ÿ�"), Tooltip("���� ���� �÷��̾� ���� �ּҰŸ��� ������ �� �ִ�."), Range(5, 15)]
    [SerializeField] private float minDis = 10.0f;
    [Header("�ִ� �Ÿ�"), Tooltip("���� ���� �÷��̾� ���� �ִ�Ÿ��� ������ �� �ִ�."), Range(6, 25)]
    [SerializeField] private float maxDis = 20.0f;
    [Header("��ȯ�� ���� ������"), Tooltip("���Ŀ� ���ҽ��������� �������� ���ϰ����� ������ ����X")]
    public List<MonsterData> monsterTypes = new List<MonsterData>(); //���� ������ ����Ʈ
    [Header("���� Ǯ ũ��"), Tooltip("MonsterPool���� ũ�⵵ �ٲ� �� �ֱ��� �ʱ� ������ ��� �����ϰ� ���� �ּ�ġ 1")]
    public int poolSize = 100; //Ǯ ũ��

    //���� Ǯ���ý����� ����ϴ� ��ųʸ�
    private Dictionary<string, MonsterPool> monsterPools = new Dictionary<string, MonsterPool>();
    //���� Ȱ��ȭ�� ���� ����Ʈ
    private List<GameObject> activeMonsters = new List<GameObject>();

    //���� ���� ���� ĳ��
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
        InitializePools(); //���� Ǯ �ʱ�ȭ
        StartCoroutine(SpawnMonsterCor()); // ���� ���� ����
    }

    //���� Ǯ �ʱ�ȭ (�� Ÿ�Ժ� Ǯ ����)
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
    //���� �������� ������ �´� ���� Ȱ��ȭ
    private void ActivateMonsters(int count)
    {
        for (int i = 0; i < count; i++)
        {
            string monsterType = monsterTypes[Random.Range(0, monsterTypes.Count)].monsterName;
            SpawnMonster(monsterType);
        }
    }
    //�÷��̾� �ֺ� �ݰ��� �������� ������ ��ġ�� ���� ����
    private Vector3 CreateAroundPlayer()
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        float distance = Random.Range(minDis, maxDis);
        return GameManager.Instance.player.transform.position
            + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * distance;
    }
    //���� ���� ��ƾ (GameManager ���)
    private IEnumerator SpawnMonsterCor()
    {
        while (GameManager.Instance.isGameStert)
        {
            int stageLevel = GameManager.Instance.stageLevel;
            yield return spawnInterval;
            int spawnCount = Random.Range(stageLevel * 1, stageLevel * 2);
            ActivateMonsters(spawnCount); //���� ���� ��� ���� Ȱ��ȭ            
        }
    }
    //���� ���� ��Ȱ��ȭ �� ��ȯ
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
