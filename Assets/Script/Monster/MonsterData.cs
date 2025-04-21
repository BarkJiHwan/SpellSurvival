using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptPbj/Monster", order = 2)]
public class MonsterData : ScriptableObject
{
    public GameObject monsterPrefab;
    public string monsterName;
    public int damage;
    public int hp;
    public float speed;
    public float rotationSpeed;
}
