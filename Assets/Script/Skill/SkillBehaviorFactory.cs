using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBehaviorFactory : MonoBehaviour
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
    public static Transform FindClosestEnemy(Vector3 position, float radius = 10f)
    {
        Collider[] hits = Physics.OverlapSphere(position, radius, LayerMask.GetMask("Monster"));

        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            float dist = Vector3.Distance(position, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = hit.transform;
            }
        }

        return closest;
    }
}
