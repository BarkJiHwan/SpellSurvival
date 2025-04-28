using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SkillFactory
{
    public static void CreateSkill(string skillName, Vector3 position, Quaternion rotation)
    {
        // 1. Ǯ���� ������Ʈ ��������
        GameObject obj = ObjectPooler.Instance.SpawnFromPool(skillName, position, rotation);
        if (obj == null)
        {
            Debug.LogError($"Failed to spawn skill: {skillName}");
            return;
        }

        // 2. Skill ������Ʈ ��������
        Skill skillComponent = obj.GetComponent<Skill>();
        if (skillComponent == null)
        {
            Debug.LogError($"Spawned prefab {skillName} does not have a Skill component!");
            Object.Destroy(obj); // ���� �ִ� ������Ʈ ����
            return;
        }

        // 3. SkillData ��������
        SkillBaseData baseData = SkillManager.Instance.GetSkillData(skillName);
        if (baseData == null)
        {
            Debug.LogError($"SkillData not found for skill: {skillName}");
            Object.Destroy(obj); // ���� �ִ� ������Ʈ ����
            return;
        }

        // 4. ActiveSkillData Ȯ��
        if (!(baseData is ActiveSkillData activeData))
        {
            Debug.LogError($"SkillData for {skillName} is not an ActiveSkillData type!");
            Object.Destroy(obj); // ���� �ִ� ������Ʈ ����
            return;
        }

        // 5. Behavior ����
        ISkillBehavior behavior = SkillBehaviorFactory.CreateBehavior(activeData.behaviorType, activeData);

        // 6. Skill �ʱ�ȭ
        skillComponent.Initialize(activeData);
        skillComponent.SetBehavior(behavior);
        activeData.ApplyBehaviorOptions(behavior);
    }
}
