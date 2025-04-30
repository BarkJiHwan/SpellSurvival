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
        set { _trackingRadius = Mathf.Max(0, value); }  // 음수 값 방지
    }

    private int _maxHits;
    public int MaxHits
    {
        get { return _maxHits; }
        set { _maxHits = Mathf.Max(0, value); }  // 음수 값 방지
    }
    public int currentHits = 0;

    public HomingBehavior(float trackingRadius, int maxHits)
    {
        this.TrackingRadius = trackingRadius;  // 동적으로 추적 범위 설정
        this.MaxHits = maxHits;  // 동적으로 최대 타겟 횟수 설정
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