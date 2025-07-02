using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillBehaviorType
{
    None,         // 기본값 (직선)
    Straight,     // 직선 발사
    Homing,       // 타겟 유도
    Boomerang,    // 부메랑
    Explosion,    // 폭발형 (이동 없음)
    Chain,        // 체인 공격 (확장용)
    Piercing,     // 관통형 (적 여러명 통과)
}
[CreateAssetMenu(fileName = "NewActiveSkill", menuName = "Skill/ActiveSkill")]
public class ActiveSkillData : SkillBaseData
{
    public SkillBehaviorType behaviorType;
    public GameObject skillPrefab;
    
    public int piercingCount;         // 관통 횟수
    public int chainCount;            // 체인 공격 횟수
    public float chainRadius;        // 체인 공격 범위
    public float explosionRadius;    // 폭발 범위
    public float explosionDelay;   // 폭발 지연 시간
    public float returnTime;       // 부메랑 반환 시간
    public float trackingRadius; // 호밍 추적 범위
    public int maxHits;         // 호밍 최대 타겟 횟수
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
