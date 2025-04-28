using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SkillType
{
    Active,
    Passive
}
public class SkillBaseData : ScriptableObject
{
    public string skillName;
    public string description;

    public float cooldown;
    public float lifeTime;
    public int baseDamage;
    public float speed;
}
