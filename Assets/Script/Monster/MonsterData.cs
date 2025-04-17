using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObj/MonsterData")]
public class MonsterData : ScriptableObject
{
    public enum MonsterRank { Normal, Epic, Elite, Boos }
    public string monsterName;
    public float hp;
    public float speed;
    public int damage;
    public bool isTargeting;    
    public GameObject monsterPrefab;
}
