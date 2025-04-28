using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBehavior : ISkillBehavior
{
    private Transform target;
    private float _trackingRadius;
    public float TrackingRadius
    {
        get { return _trackingRadius; }
        set { _trackingRadius = Mathf.Max(0, value); }  // ���� �� ����
    }

    private int _maxHits;
    public int MaxHits
    {
        get { return _maxHits; }
        set { _maxHits = Mathf.Max(0, value); }  // ���� �� ����
    }
    private int currentHits = 0;

    public HomingBehavior(float trackingRadius, int maxHits)
    {
        this.TrackingRadius = trackingRadius;  // �������� ���� ���� ����
        this.MaxHits = maxHits;  // �������� �ִ� Ÿ�� Ƚ�� ����
        target = FindClosestEnemy();
    }

    public void UpdateBehavior(Skill skill)
    {
        if (target == null)
        {
            skill.ReturnToPool();
            return;
        }

        Vector3 direction = (target.position - skill.transform.position).normalized;
        skill.transform.Translate(direction * skill.speed * Time.deltaTime);

        if (Vector3.Distance(skill.transform.position, target.position) < 0.5f)
        {
            currentHits++;
            if (currentHits >= MaxHits)
            {
                skill.ReturnToPool();
            }
        }
    }

    private Transform FindClosestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(Vector3.zero, TrackingRadius, LayerMask.GetMask("Monster"));

        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            float dist = Vector3.Distance(Vector3.zero, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = hit.transform;
            }
        }

        return closest;
    }
}
