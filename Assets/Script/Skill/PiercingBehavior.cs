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
        RemainingPierces = piercingCount;  // �������� ���� Ƚ�� ����
    }

    public void UpdateBehavior(Skill skill)
    {
        // ���� ã�� �������� �̵�
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

            // ���� ī��Ʈ ���� �� ����
            if (RemainingPierces <= 0)
            {
                skill.ReturnToPool();
            }
        }
    }
}
