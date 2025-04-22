using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool
{
    private List<GameObject> monsterObjPool = new List<GameObject>();
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
            monsterObjPool.Add(monster);
        }
    }

    public GameObject Spawn(Vector3 position)
    {
        if (monsterObjPool.Count > 0)
        {
            GameObject monster = monsterObjPool[monsterObjPool.Count - 1];
            monsterObjPool.RemoveAt(monsterObjPool.Count - 1);
            monster.transform.position = position;
            monster.SetActive(true);
            return monster;
        }
        return null;
    }
    public void Reture(GameObject monster)
    {
        monster.SetActive(false);
        monsterObjPool.Add(monster);
    }

}
