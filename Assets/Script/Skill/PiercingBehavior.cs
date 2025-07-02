using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingBehavior : ISkillBehavior
{
    private bool initialized = false;
    private Vector3 moveDirection;
    private Transform target;

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
    public PiercingBehavior(int piercingCount)
    {
        RemainingPierces = piercingCount;  // �������� ���� Ƚ�� ����
    }

    public void UpdateBehavior(Skill skill)
    {
        // ���� ã�� �������� �̵�
        if (!initialized)
        {
            initialized = true;
            target = SkillBehaviorFactory.FindClosestEnemy(skill.transform.position, 100);
            if (target != null)
            {
                moveDirection = (target.position - skill.transform.position).normalized;
            }
            else
            {
                moveDirection = Vector3.forward;
            }
        }
        skill.transform.Translate(moveDirection * skill.Speed * Time.deltaTime);
    }

    public void OnHit(Skill skill, Collision collision)
    {
        if (collision.transform.CompareTag("Monster"))
        {
            RemainingPierces--;
            // ���� ī��Ʈ ���� �� ����
            if (RemainingPierces <= 0)
            {
                target = null;
                skill.ReturnToPool();
            }
        }
    }
}
