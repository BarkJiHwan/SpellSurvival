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
    public int currentHits = 0;

    public HomingBehavior(float trackingRadius, int maxHits)
    {
        this.TrackingRadius = trackingRadius;  // �������� ���� ���� ����
        this.MaxHits = maxHits;  // �������� �ִ� Ÿ�� Ƚ�� ����
    }

    public void UpdateBehavior(Skill skill)
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            target = SkillBehaviorFactory.FindClosestEnemy(skill.transform.position, TrackingRadius);
            if (target == null)
            {
                skill.speed = 0;
                return;
            }
            else
            {
                skill.speed = skill.skillData.speed;
            }
        }
        Vector3 direction = (target.position - skill.transform.position).normalized;
        skill.transform.Translate(direction * skill.speed * Time.deltaTime);
    }
    public void OnHit(Skill skill, Collision collision)
    {
        if (collision.transform.CompareTag("Monster"))
        {
            currentHits++;
            if (currentHits >= MaxHits)
            {
                skill.ReturnToPool();
            }
            else
            {
                target = null;
            }
        }
    }    
}