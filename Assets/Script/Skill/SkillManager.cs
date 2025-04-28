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
                    skillLevels[activeSkill.skillName] = 1; // 기본 스킬 레벨 1
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
    /// 액티브 스킬 오브젝트 풀에 미리 등록
    /// </summary>
    private void PrepareSkillObject(ActiveSkillData activeSkill)
    {
        // ObjectPooler 쪽에 등록하는 부분
        ObjectPooler.Instance.CreatePool(activeSkill.skillName, activeSkill.skillPrefab, 5, 20);
    }

    /// <summary>
    /// 패시브 스킬 효과 적용 (나중에 세부구현)
    /// </summary>
    private void ApplyPassiveEffect(PassiveSkillData passiveSkill)
    {
        // 예시: 전체 액티브 스킬에 효과 적용
        foreach (var activeSkill in activeSkillDict.Values)
        {
            activeSkill.baseDamage = Mathf.RoundToInt(activeSkill.baseDamage * passiveSkill.damageMultiplier);
            activeSkill.speed *= passiveSkill.speedMultiplier;
            // 쿨다운 감소 같은 추가 구현 가능
        }

        Debug.Log($"PassiveSkill {passiveSkill.skillName} applied: DMG x{passiveSkill.damageMultiplier}, SPD x{passiveSkill.speedMultiplier}");
    }

    /// <summary>
    /// SkillName으로 ActiveSkillData 조회
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
    /// SkillName으로 PassiveSkillData 조회
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

    // 전체 가져오기 (필요할 경우)
    public IEnumerable<ActiveSkillData> GetAllActiveSkills() => activeSkillDict.Values;
    public IEnumerable<PassiveSkillData> GetAllPassiveSkills() => passiveSkillDict.Values;
    /// <summary>
    /// 스킬 습득 및 초기 레벨 설정
    /// </summary>
    public void LearnSkill(SkillBaseData skillData)
    {
        if (skillData is ActiveSkillData activeSkill)
        {
            if (!activeSkillDict.ContainsKey(activeSkill.skillName))
            {
                activeSkillDict.Add(activeSkill.skillName, activeSkill);
                skillLevels[activeSkill.skillName] = 1; // 레벨 1 시작
                PrepareSkillObject(activeSkill);
            }
        }
        else if (skillData is PassiveSkillData passiveSkill)
        {
            if (!passiveSkillDict.ContainsKey(passiveSkill.skillName))
            {
                passiveSkillDict.Add(passiveSkill.skillName, passiveSkill);
                skillLevels[passiveSkill.skillName] = 1; // 레벨 1 시작
                ApplyPassiveEffect(passiveSkill);
            }
        }
    }

    /// <summary>
    /// 스킬 레벨업
    /// </summary>
    public void LevelUpSkill(string skillName)
    {
        if (skillLevels.ContainsKey(skillName))
        {
            skillLevels[skillName]++;
            Debug.Log($"Skill {skillName} leveled up! New Level: {skillLevels[skillName]}");

            // 레벨업 효과 적용
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
    /// 액티브 스킬 레벨업 강화
    /// </summary>
    private void UpgradeActiveSkill(ActiveSkillData skill)
    {
        // 예시: 데미지 +10%, 속도 +5%
        int level = skillLevels[skill.skillName];
        skill.baseDamage = Mathf.RoundToInt(skill.baseDamage * (1 + 0.1f * level)); // 데미지 증가
        skill.speed *= 1 + 0.05f * level; // 속도 증가

        // 예: 특정 레벨에서 새로운 효과 추가
        if (level >= 5)
        {
            skill.cooldown *= 0.9f;  // 5레벨부터 쿨다운 감소
            Debug.Log($"{skill.skillName} has reduced cooldown at level {level}.");
        }
    }

    /// <summary>
    /// 현재 스킬 레벨 반환
    /// </summary>
    public int GetSkillLevel(string skillName)
    {
        return skillLevels.TryGetValue(skillName, out var level) ? level : 0;
    }
}
