using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBehavior : ISkillBehavior
{
    private Transform currentTarget;
    private int _chainCount;  // ü�� Ƚ�� (�ʵ�)
    private float _chainRadius;  // ü�� ���� (�ʵ�)

    // ü�� Ƚ�� ������Ƽ
    public int ChainCount
    {
        get { return _chainCount; }
        set { _chainCount = Mathf.Max(0, value); }  // ���� �� ����
    }

    // ü�� ���� ������Ƽ
    public float ChainRadius
    {
        get { return _chainRadius; }
        set { _chainRadius = Mathf.Max(0, value); }  // ���� �� ����
    }

    // ������
    public ChainBehavior(int chainCount, float chainRadius)
    {
        this._chainCount = Mathf.Max(0, chainCount);  // ���� �� ����
        this._chainRadius = Mathf.Max(0, chainRadius);  // ���� �� ����
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

        // ���� �Ÿ� �̳� �����ϸ� ���� Ÿ�� ã��
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
