using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PassiveEffectType
{
    IncreaseDamage,
    DecreaseCooldown,
    IncreaseSpeed,
    IncreaseLifeTime
}
[CreateAssetMenu(fileName = "NewPassiveSkill", menuName = "Skill/PassiveSkill")]
public class PassiveSkillData : SkillBaseData
{
    public PassiveEffectType effectType;
    public float effectValue;
    [Header("Passive Effects")]
    public float damageMultiplier = 1f;
    public float speedMultiplier = 1f;
    public float cooldownReduction = 0f;

}
