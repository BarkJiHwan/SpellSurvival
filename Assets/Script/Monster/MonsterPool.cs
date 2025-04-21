using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool
{
    private List<GameObject> activePool = new List<GameObject>();
    private GameObject prefab;
    private Transform parentTransform;

    public MonsterPool(GameObject prefab, int poolSize, Transform parent)
    {
        this.prefab = prefab;
        this.parentTransform = parent;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject monster = Object.Instantiate(prefab, parentTransform);
            monster.SetActive(false);
            activePool.Add(monster);
        }
    }

    public GameObject Spawn(Vector3 position)
    {
        if (activePool.Count > 0)
        {
            GameObject monster = activePool[activePool.Count - 1];
            activePool.RemoveAt(activePool.Count - 1);
            monster.transform.position = position;
            monster.SetActive(true);
            return monster;
        }
        return null;
    }
    public void Reture(GameObject monster)
    {
        monster.SetActive(false);
        activePool.Add(monster);
    }

}
