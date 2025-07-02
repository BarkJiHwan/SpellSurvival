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
        set { _returnTime = Mathf.Max(0, value); }  // 음수 값은 허용하지 않음
    }
    public BoomerangBehavior(float returnTime)
    {
        this.ReturnTime = returnTime;  // 동적으로 반환 시간 설정
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
