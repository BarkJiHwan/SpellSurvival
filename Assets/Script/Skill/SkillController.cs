using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    private List<(string skillName, float cooldown)> skillCooldowns = new List<(string, float)>();

    private SkillManager skillManager;

    private void Start()
    {
        skillManager = SkillManager.Instance; // SkillManager 캐싱

        // 기본 스킬 자동 사용 테스트 (예시)
        foreach (var skillName in skillManager.defaultSkills)
        {
            RegisterAutoSkill(skillName.skillName);
        }
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        // 쿨다운 갱신 및 자동 발사 처리
        UpdateCooldowns(deltaTime);
    }

    /// <summary>
    /// 쿨다운 갱신 및 자동 발사 처리
    /// </summary>
    private void UpdateCooldowns(float deltaTime)
    {
        // 쿨다운이 0인 스킬만 발사
        foreach (var skill in skillCooldowns.ToList()) // ToList()를 사용하여 안전한 순회
        {
            if (skill.cooldown > 0)
            {
                // 쿨다운 갱신
                skillCooldowns[skillCooldowns.IndexOf(skill)] = (skill.skillName, skill.cooldown - deltaTime);
            }
            else
            {
                // 쿨다운이 끝나면 스킬 발사
                UseSkill(skill.skillName);
            }
        }
    }

    /// <summary>
    /// 스킬을 수동으로 한번 등록해두기
    /// </summary>
    public void RegisterAutoSkill(string skillName)
    {
        if (!skillCooldowns.Any(s => s.skillName == skillName))
        {
            skillCooldowns.Add((skillName, 0f)); // 바로 사용 가능하게 초기화
        }
    }

    /// <summary>
    /// 스킬 사용 (내부용)
    /// </summary>
    private void UseSkill(string skillName)
    {
        ActiveSkillData skillData = skillManager.GetSkillData(skillName);
        if (skillData == null)
        {
            Debug.LogError($"SkillData not found: {skillName}");
            return;
        }

        // 스킬 위치 및 회전 수정
        SkillFactory.CreateSkill(skillName, transform.position, Quaternion.identity);

        // 쿨다운을 갱신
        UpdateSkillCooldown(skillName, skillData.cooldown);
    }

    /// <summary>
    /// 쿨다운 갱신
    /// </summary>
    private void UpdateSkillCooldown(string skillName, float cooldown)
    {
        int index = skillCooldowns.FindIndex(s => s.skillName == skillName);
        if (index >= 0)
        {
            skillCooldowns[index] = (skillName, cooldown);
        }
    }

    /// <summary>
    /// 쿨다운 강제 리셋
    /// </summary>
    public void ResetSkillCooldown(string skillName)
    {
        int index = skillCooldowns.FindIndex(s => s.skillName == skillName);
        if (index >= 0)
        {
            skillCooldowns[index] = (skillName, 0f);
        }
    }

    /// <summary>
    /// 현재 쿨타임 조회
    /// </summary>
    public float GetSkillCooldown(string skillName)
    {
        var skill = skillCooldowns.FirstOrDefault(s => s.skillName == skillName);
        return skill.Equals(default) ? 0f : skill.cooldown;
    }
}
