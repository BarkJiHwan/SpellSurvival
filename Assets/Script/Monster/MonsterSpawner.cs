using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public PlayerController player;

    float minDis = 5.0f;
    float maxDis = 20.0f;
    public int poolSize = 1000;
    private Queue<GameObject> pool = new Queue<GameObject>();
    public GameObject monsterPrefab;
    WaitForSeconds seconds = new WaitForSeconds(1);
    public float timer;
    void Start()
    {        
        for (int i = 0; i < poolSize; i++)
        {
            GameObject monster = Instantiate(monsterPrefab, transform);
            monster.SetActive(false);
            pool.Enqueue(monster);
        }
        StartCoroutine(SpawnMonsterCor());
    }
    void SpawnMonster()
    {
        // 1. Ǯ���� ��Ȱ��ȭ�� ���͸� ã��
        GameObject monster = pool.Dequeue();
     
        // 2. �÷��̾� ���� ���� ��ġ ���
        float angle = Random.Range(0, Mathf.PI * 2);
        float distance = Random.Range(minDis, maxDis);
        Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * distance;
        Vector3 spawnPos = player.transform.position + offset;

        // 3. ���� ��ġ �̵� �� Ȱ��ȭ
        monster.transform.position = spawnPos;
        monster.SetActive(true);
    }
    public void ReturnMonster(GameObject monster)
    {
        monster.SetActive(false);
        pool.Enqueue(monster);
    }

    IEnumerator SpawnMonsterCor()
    {
        while (true)
        {
            SpawnMonster();
            yield return seconds;
        }
    }
}
