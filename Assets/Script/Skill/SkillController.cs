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
        skillManager = SkillManager.Instance; // SkillManager ĳ��

        // �⺻ ��ų �ڵ� ��� �׽�Ʈ (����)
        foreach (var skillName in skillManager.defaultSkills)
        {
            RegisterAutoSkill(skillName.skillName);
        }
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        // ��ٿ� ���� �� �ڵ� �߻� ó��
        UpdateCooldowns(deltaTime);
    }

    /// <summary>
    /// ��ٿ� ���� �� �ڵ� �߻� ó��
    /// </summary>
    private void UpdateCooldowns(float deltaTime)
    {
        // ��ٿ��� 0�� ��ų�� �߻�
        foreach (var skill in skillCooldowns.ToList()) // ToList()�� ����Ͽ� ������ ��ȸ
        {
            if (skill.cooldown > 0)
            {
                // ��ٿ� ����
                skillCooldowns[skillCooldowns.IndexOf(skill)] = (skill.skillName, skill.cooldown - deltaTime);
            }
            else
            {
                // ��ٿ��� ������ ��ų �߻�
                UseSkill(skill.skillName);
            }
        }
    }

    /// <summary>
    /// ��ų�� �������� �ѹ� ����صα�
    /// </summary>
    public void RegisterAutoSkill(string skillName)
    {
        if (!skillCooldowns.Any(s => s.skillName == skillName))
        {
            skillCooldowns.Add((skillName, 0f)); // �ٷ� ��� �����ϰ� �ʱ�ȭ
        }
    }

    /// <summary>
    /// ��ų ��� (���ο�)
    /// </summary>
    private void UseSkill(string skillName)
    {
        ActiveSkillData skillData = skillManager.GetSkillData(skillName);
        if (skillData == null)
        {
            Debug.LogError($"SkillData not found: {skillName}");
            return;
        }

        // ��ų ��ġ �� ȸ�� ����
        SkillFactory.CreateSkill(skillName, transform.position, Quaternion.identity);

        // ��ٿ��� ����
        UpdateSkillCooldown(skillName, skillData.cooldown);
    }

    /// <summary>
    /// ��ٿ� ����
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
    /// ��ٿ� ���� ����
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
    /// ���� ��Ÿ�� ��ȸ
    /// </summary>
    public float GetSkillCooldown(string skillName)
    {
        var skill = skillCooldowns.FirstOrDefault(s => s.skillName == skillName);
        return skill.Equals(default) ? 0f : skill.cooldown;
    }
}
