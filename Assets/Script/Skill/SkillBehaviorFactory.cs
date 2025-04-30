using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBehaviorFactory
{
    public static ISkillBehavior CreateBehavior(SkillBehaviorType type, ActiveSkillData activeData)
    {
        switch (type)
        {
            case SkillBehaviorType.Straight: 
                return new StraightBehavior();
            case SkillBehaviorType.Homing:
                return new HomingBehavior(activeData.trackingRadius, activeData.maxHits);
            case SkillBehaviorType.Boomerang:
                return new BoomerangBehavior(activeData.returnTime);
            case SkillBehaviorType.Explosion:
                return new ExplosionBehavior(activeData.explosionRadius, activeData.explosionDelay);
            case SkillBehaviorType.Chain:
                return new ChainBehavior(activeData.chainCount, activeData.chainRadius);
            case SkillBehaviorType.Piercing:
                return new PiercingBehavior(activeData.piercingCount);
            default: 
                return new StraightBehavior();
        }
    }
    public static Transform FindClosestEnemy(Vector3 position, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(position, radius, LayerMask.GetMask("Monster"));

        Transform closestMonster = null;
        float minDist = radius;

        foreach (var hit in hits)
        {
            float dist = Vector3.Distance(position, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestMonster = hit.transform;
            }
        }
        return closestMonster;
    }
}
