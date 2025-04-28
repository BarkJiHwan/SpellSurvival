using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    [SerializeField] private Dictionary<string, float> skillCooldowns = new Dictionary<string, float>();
    //private HashSet<string> autoSkills = new HashSet<string>(); // �ڵ� �߻��� ��ų ����Ʈ (HashSet ���)
    private void Start()
    {
        // �⺻ ��ų �ڵ� ��� �׽�Ʈ (����)
        string skillName = "FireBalt"; // �⺻ ��ų �̸�
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
                // ��ٿ��� �������� �ڵ� �߻�                
                if (skillCooldowns[skillName] <= 0/* && autoSkills.Contains(skillName)*/)
                {
                    UseSkill(skillName);
                }
            }
        }
    }

    /// <summary>
    /// ��ų�� �������� �ѹ� ����صα�
    /// </summary>
    public void RegisterAutoSkill(string skillName)
    {
        //if (!autoSkills.Contains(skillName))
        //{
        //    autoSkills.Add(skillName);
            skillCooldowns[skillName] = 0f; // �ٷ� ��� �����ϰ� �ʱ�ȭ
        //}
    }

    /// <summary>
    /// ��ų ��� (���ο�)
    /// </summary>
    private void UseSkill(string skillName)
    {
        ActiveSkillData skillData = SkillManager.Instance.GetSkillData(skillName);
        if (skillData == null)
        {
            Debug.LogError($"SkillData not found: {skillName}");
            return;
        }

        // ��ų ��ġ �� ȸ�� ����
        SkillFactory.CreateSkill(skillName, transform.position, Quaternion.identity);
        skillCooldowns[skillName] = skillData.cooldown;
    }

    /// <summary>
    /// ��ٿ� ���� ����
    /// </summary>
    public void ResetSkillCooldown(string skillName)
    {
        if (skillCooldowns.ContainsKey(skillName))
        {
            skillCooldowns[skillName] = 0f;
        }
    }

    /// <summary>
    /// ���� ��Ÿ�� ��ȸ
    /// </summary>
    public float GetSkillCooldown(string skillName)
    {
        return skillCooldowns.TryGetValue(skillName, out float cooldown) ? cooldown : 0f;
    }
}
