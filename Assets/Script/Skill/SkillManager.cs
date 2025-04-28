using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private static SkillManager instance;

    private Dictionary<string, ActiveSkillData> activeSkillDict = new Dictionary<string, ActiveSkillData>();
    private Dictionary<string, PassiveSkillData> passiveSkillDict = new Dictionary<string, PassiveSkillData>();
    private Dictionary<string, int> skillLevels = new Dictionary<string, int>();

    public List<SkillBaseData> defaultSkills;

    public static SkillManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }


        RegisterDefaultSkills();
    }
    private void RegisterDefaultSkills()
    {
        foreach (var skill in defaultSkills)
        {
            if (skill is ActiveSkillData activeSkill)
            {
                if (!activeSkillDict.ContainsKey(activeSkill.skillName))
                {
                    activeSkillDict.Add(activeSkill.skillName, activeSkill);
                    skillLevels[activeSkill.skillName] = 1; // �⺻ ��ų ���� 1
                    PrepareSkillObject(activeSkill);
                    Debug.Log($"Active Skill {activeSkill.skillName} registered.");
                }
            }
            else if (skill is PassiveSkillData passiveSkill)
            {
                if (!passiveSkillDict.ContainsKey(passiveSkill.skillName))
                {
                    passiveSkillDict.Add(passiveSkill.skillName, passiveSkill);
                    skillLevels[passiveSkill.skillName] = 1;
                    ApplyPassiveEffect(passiveSkill);
                    Debug.Log($"Passive Skill {passiveSkill.skillName} registered.");
                }
            }
        }
    }

    /// <summary>
    /// ��Ƽ�� ��ų ������Ʈ Ǯ�� �̸� ���
    /// </summary>
    private void PrepareSkillObject(ActiveSkillData activeSkill)
    {
        // ObjectPooler �ʿ� ����ϴ� �κ�
        ObjectPooler.Instance.CreatePool(activeSkill.skillName, activeSkill.skillPrefab, 5, 20);
    }

    /// <summary>
    /// �нú� ��ų ȿ�� ���� (���߿� ���α���)
    /// </summary>
    private void ApplyPassiveEffect(PassiveSkillData passiveSkill)
    {
        // ����: ��ü ��Ƽ�� ��ų�� ȿ�� ����
        foreach (var activeSkill in activeSkillDict.Values)
        {
            activeSkill.baseDamage = Mathf.RoundToInt(activeSkill.baseDamage * passiveSkill.damageMultiplier);
            activeSkill.speed *= passiveSkill.speedMultiplier;
            // ��ٿ� ���� ���� �߰� ���� ����
        }

        Debug.Log($"PassiveSkill {passiveSkill.skillName} applied: DMG x{passiveSkill.damageMultiplier}, SPD x{passiveSkill.speedMultiplier}");
    }

    /// <summary>
    /// SkillName���� ActiveSkillData ��ȸ
    /// </summary>
    public ActiveSkillData GetSkillData(string skillName)
    {
        if (activeSkillDict.TryGetValue(skillName, out ActiveSkillData data))
        {
            return data;
        }
        else
        {
            Debug.LogError($"ActiveSkillData not found for: {skillName}");
            return null;
        }
    }

    /// <summary>
    /// SkillName���� PassiveSkillData ��ȸ
    /// </summary>
    public PassiveSkillData GetPassiveSkillData(string skillName)
    {
        if (passiveSkillDict.TryGetValue(skillName, out PassiveSkillData data))
        {
            return data;
        }
        else
        {
            Debug.LogError($"PassiveSkillData not found for: {skillName}");
            return null;
        }
    }

    // ��ü �������� (�ʿ��� ���)
    public IEnumerable<ActiveSkillData> GetAllActiveSkills() => activeSkillDict.Values;
    public IEnumerable<PassiveSkillData> GetAllPassiveSkills() => passiveSkillDict.Values;
    /// <summary>
    /// ��ų ���� �� �ʱ� ���� ����
    /// </summary>
    public void LearnSkill(SkillBaseData skillData)
    {
        if (skillData is ActiveSkillData activeSkill)
        {
            if (!activeSkillDict.ContainsKey(activeSkill.skillName))
            {
                activeSkillDict.Add(activeSkill.skillName, activeSkill);
                skillLevels[activeSkill.skillName] = 1; // ���� 1 ����
                PrepareSkillObject(activeSkill);
            }
        }
        else if (skillData is PassiveSkillData passiveSkill)
        {
            if (!passiveSkillDict.ContainsKey(passiveSkill.skillName))
            {
                passiveSkillDict.Add(passiveSkill.skillName, passiveSkill);
                skillLevels[passiveSkill.skillName] = 1; // ���� 1 ����
                ApplyPassiveEffect(passiveSkill);
            }
        }
    }

    /// <summary>
    /// ��ų ������
    /// </summary>
    public void LevelUpSkill(string skillName)
    {
        if (skillLevels.ContainsKey(skillName))
        {
            skillLevels[skillName]++;
            Debug.Log($"Skill {skillName} leveled up! New Level: {skillLevels[skillName]}");

            // ������ ȿ�� ����
            if (activeSkillDict.TryGetValue(skillName, out var activeSkill))
            {
                UpgradeActiveSkill(activeSkill);
            }
            else if (passiveSkillDict.TryGetValue(skillName, out var passiveSkill))
            {
                ApplyPassiveEffect(passiveSkill);
            }
        }
        else
        {
            Debug.LogError($"Skill {skillName} not found in skill levels!");
        }
    }

    /// <summary>
    /// ��Ƽ�� ��ų ������ ��ȭ
    /// </summary>
    private void UpgradeActiveSkill(ActiveSkillData skill)
    {
        // ����: ������ +10%, �ӵ� +5%
        int level = skillLevels[skill.skillName];
        skill.baseDamage = Mathf.RoundToInt(skill.baseDamage * (1 + 0.1f * level)); // ������ ����
        skill.speed *= 1 + 0.05f * level; // �ӵ� ����

        // ��: Ư�� �������� ���ο� ȿ�� �߰�
        if (level >= 5)
        {
            skill.cooldown *= 0.9f;  // 5�������� ��ٿ� ����
            Debug.Log($"{skill.skillName} has reduced cooldown at level {level}.");
        }
    }

    /// <summary>
    /// ���� ��ų ���� ��ȯ
    /// </summary>
    public int GetSkillLevel(string skillName)
    {
        return skillLevels.TryGetValue(skillName, out var level) ? level : 0;
    }
}
