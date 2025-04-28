using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    [SerializeField] private Dictionary<string, float> skillCooldowns = new Dictionary<string, float>();
    //private HashSet<string> autoSkills = new HashSet<string>(); // 자동 발사할 스킬 리스트 (HashSet 사용)
    private void Start()
    {
        // 기본 스킬 자동 사용 테스트 (예시)
        string skillName = "FireBalt"; // 기본 스킬 이름
        UseSkill(skillName);
    }
    private void Update()
    {
        float deltaTime = Time.deltaTime;
        List<string> skillNames = new List<string>(skillCooldowns.Keys);
        foreach (var skillName in skillNames)
        {
            if (skillCooldowns[skillName] > 0)
            {
                skillCooldowns[skillName] -= deltaTime;
                // 쿨다운이 끝났으면 자동 발사                
                if (skillCooldowns[skillName] <= 0/* && autoSkills.Contains(skillName)*/)
                {
                    UseSkill(skillName);
                }
            }
        }
    }

    /// <summary>
    /// 스킬을 수동으로 한번 등록해두기
    /// </summary>
    public void RegisterAutoSkill(string skillName)
    {
        //if (!autoSkills.Contains(skillName))
        //{
        //    autoSkills.Add(skillName);
            skillCooldowns[skillName] = 0f; // 바로 사용 가능하게 초기화
        //}
    }

    /// <summary>
    /// 스킬 사용 (내부용)
    /// </summary>
    private void UseSkill(string skillName)
    {
        ActiveSkillData skillData = SkillManager.Instance.GetSkillData(skillName);
        if (skillData == null)
        {
            Debug.LogError($"SkillData not found: {skillName}");
            return;
        }

        // 스킬 위치 및 회전 수정
        SkillFactory.CreateSkill(skillName, transform.position, Quaternion.identity);
        skillCooldowns[skillName] = skillData.cooldown;
    }

    /// <summary>
    /// 쿨다운 강제 리셋
    /// </summary>
    public void ResetSkillCooldown(string skillName)
    {
        if (skillCooldowns.ContainsKey(skillName))
        {
            skillCooldowns[skillName] = 0f;
        }
    }

    /// <summary>
    /// 현재 쿨타임 조회
    /// </summary>
    public float GetSkillCooldown(string skillName)
    {
        return skillCooldowns.TryGetValue(skillName, out float cooldown) ? cooldown : 0f;
    }
}
