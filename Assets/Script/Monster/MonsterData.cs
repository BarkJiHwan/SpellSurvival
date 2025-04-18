using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptPbj/Monster")]
public class MonsterData : ScriptableObject
{
    public string MonsterName;
    public int damage;
    public int hp;
    public float speed;
    
}
