using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBehavior : ISkillBehavior
{
    private Transform currentTarget;
    private int _chainCount;  // 체인 횟수 (필드)
    private float _chainRadius;  // 체인 범위 (필드)

    // 체인 횟수 프로퍼티
    public int ChainCount
    {
        get { return _chainCount; }
        set { _chainCount = Mathf.Max(0, value); }  // 음수 값 방지
    }

    // 체인 범위 프로퍼티
    public float ChainRadius
    {
        get { return _chainRadius; }
        set { _chainRadius = Mathf.Max(0, value); }  // 음수 값 방지
    }

    // 생성자
    public ChainBehavior(int chainCount, float chainRadius)
    {
        this._chainCount = Mathf.Max(0, chainCount);  // 음수 값 방지
        this._chainRadius = Mathf.Max(0, chainRadius);  // 음수 값 방지
        currentTarget = FindClosestEnemy();
    }


    public void UpdateBehavior(Skill skill)
    {
        if (currentTarget == null)
        {
            currentTarget = FindClosestEnemy();
            if (currentTarget == null)
            {
                skill.ReturnToPool();
                return;
            }
        }

        Vector3 direction = (currentTarget.position - skill.transform.position).normalized;
        skill.transform.position += direction * skill.speed * Time.deltaTime;

        // 일정 거리 이내 접근하면 다음 타겟 찾기
        if (Vector3.Distance(skill.transform.position, currentTarget.position) < 0.5f)
        {
            ChainCount--;
            if (ChainCount <= 0)
            {
                skill.ReturnToPool();
                return;
            }

            currentTarget = FindClosestEnemy(currentTarget);
        }
    }

    private Transform FindClosestEnemy(Transform exclude = null)
    {
        return SkillBehaviorFactory.FindClosestEnemy(exclude != null ? exclude.position : Vector3.zero);
    }
}
