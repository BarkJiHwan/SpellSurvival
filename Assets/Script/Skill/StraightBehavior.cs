using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBehavior : ISkillBehavior
{
    public void UpdateBehavior(Skill skill)
    {
        skill.transform.Translate(Vector3.forward * skill.speed * Time.deltaTime);
    }

}
