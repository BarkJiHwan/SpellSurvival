using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBehavior : ISkillBehavior
{
    private float elapsed = 0f;
    private float _returnTime;
    public float ReturnTime
    {
        get { return _returnTime; }
        set { _returnTime = Mathf.Max(0, value); }  // ���� ���� ������� ����
    }
    public BoomerangBehavior(float returnTime)
    {
        this.ReturnTime = returnTime;  // �������� ��ȯ �ð� ����
    }

    public void UpdateBehavior(Skill skill)
    {
        elapsed += Time.deltaTime;
        if (elapsed < (ReturnTime / 2))
        {
            skill.transform.Translate(Vector3.forward * skill.Speed * Time.deltaTime);
        }
        else
        {
            skill.transform.Translate(-Vector3.forward * skill.Speed * Time.deltaTime);
        }
    }
}
