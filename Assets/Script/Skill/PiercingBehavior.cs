using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingBehavior : ISkillBehavior
{
    private int _remainingPierces;

    public int RemainingPierces
    {
        get { return _remainingPierces; }
        set
        {
            if (value < 0)
                _remainingPierces = 0;
            else
                _remainingPierces = value;
        }
    }
    private float radius = 10f;
    public PiercingBehavior(int piercingCount)
    {
        RemainingPierces = piercingCount;  // 동적으로 관통 횟수 설정
    }

    public void UpdateBehavior(Skill skill)
    {
        // 적을 찾는 방향으로 이동
        Transform target = SkillBehaviorFactory.FindClosestEnemy(skill.transform.position, radius);
        if (target != null)
        {
            Vector3 direction = (target.position - skill.transform.position).normalized;
            skill.transform.Translate(direction * skill.speed * Time.deltaTime);
        }
        else
        {
            skill.transform.Translate(Vector3.forward * skill.speed * Time.deltaTime);
        }
    }

    public void OnHit(Skill skill, Collision collision)
    {
        if (collision.transform.CompareTag("Monster"))
        {
            RemainingPierces--;

            // 관통 카운트 소진 시 리턴
            if (RemainingPierces <= 0)
            {
                skill.ReturnToPool();
            }
        }
    }
}
