using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SkillFactory
{
    public static void CreateSkill(string skillName, Vector3 position, Quaternion rotation)
    {
        // 1. 풀에서 오브젝트 가져오기
        GameObject obj = ObjectPooler.Instance.SpawnFromPool(skillName, position, rotation);
        if (obj == null)
        {
            Debug.LogError($"Failed to spawn skill: {skillName}");
            return;
        }

        // 2. Skill 컴포넌트 가져오기
        Skill skillComponent = obj.GetComponent<Skill>();
        if (skillComponent == null)
        {
            Debug.LogError($"Spawned prefab {skillName} does not have a Skill component!");
            Object.Destroy(obj); // 문제 있는 오브젝트 정리
            return;
        }

        // 3. SkillData 가져오기
        SkillBaseData baseData = SkillManager.Instance.GetSkillData(skillName);
        if (baseData == null)
        {
            Debug.LogError($"SkillData not found for skill: {skillName}");
            Object.Destroy(obj); // 문제 있는 오브젝트 정리
            return;
        }

        // 4. ActiveSkillData 확인
        if (!(baseData is ActiveSkillData activeData))
        {
            Debug.LogError($"SkillData for {skillName} is not an ActiveSkillData type!");
            Object.Destroy(obj); // 문제 있는 오브젝트 정리
            return;
        }

        // 5. Behavior 생성
        ISkillBehavior behavior = SkillBehaviorFactory.CreateBehavior(activeData.behaviorType, activeData);

        // 6. Skill 초기화
        skillComponent.Initialize(activeData);
        skillComponent.SetBehavior(behavior);
        activeData.ApplyBehaviorOptions(behavior);
    }
}
