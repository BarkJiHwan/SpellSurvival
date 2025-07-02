using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillBehaviorType
{
    None,         // �⺻�� (����)
    Straight,     // ���� �߻�
    Homing,       // Ÿ�� ����
    Boomerang,    // �θ޶�
    Explosion,    // ������ (�̵� ����)
    Chain,        // ü�� ���� (Ȯ���)
    Piercing,     // ������ (�� ������ ���)
}
[CreateAssetMenu(fileName = "NewActiveSkill", menuName = "Skill/ActiveSkill")]
public class ActiveSkillData : SkillBaseData
{
    public SkillBehaviorType behaviorType;
    public GameObject skillPrefab;
    
    public int piercingCount;         // ���� Ƚ��
    public int chainCount;            // ü�� ���� Ƚ��
    public float chainRadius;        // ü�� ���� ����
    public float explosionRadius;    // ���� ����
    public float explosionDelay;   // ���� ���� �ð�
    public float returnTime;       // �θ޶� ��ȯ �ð�
    public float trackingRadius; // ȣ�� ���� ����
    public int maxHits;         // ȣ�� �ִ� Ÿ�� Ƚ��
    public void ApplyBehaviorOptions(ISkillBehavior behavior)
    {
        switch (behaviorType)
        {
            case SkillBehaviorType.Piercing:
                if (behavior is PiercingBehavior piercing)
                {
                    piercing.RemainingPierces = piercingCount;
                }
                break;

            case SkillBehaviorType.Chain:
                if (behavior is ChainBehavior chain)
                {
                    chain.ChainCount = chainCount;
                    chain.ChainRadius = chainRadius;
                }
                break;

            case SkillBehaviorType.Explosion:
                if (behavior is ExplosionBehavior explosion)
                {
                    explosion.ExplosionRadius = explosionRadius;
                    explosion.ExplosionDelay = explosionDelay;
                }
                break;

            case SkillBehaviorType.Boomerang:
                if (behavior is BoomerangBehavior boomerang)
                {
                    boomerang.ReturnTime = returnTime;
                }
                break;

            case SkillBehaviorType.Homing:
                if (behavior is HomingBehavior homing)
                {
                    homing.TrackingRadius = trackingRadius;
                    homing.MaxHits = maxHits;
                }
                break;
        }
    }
}
