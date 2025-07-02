using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChainBehavior : ISkillBehavior
{
    private int _chainCount;  // 체인 횟수 (필드)
    private float _chainRadius;  // 체인 범위 (필드)
    private bool hasHit = false;
    private bool initialized = false;
    private Vector3 moveDirection;
    private LineRenderer lineRenderer;
    // 체인 횟수 프로퍼티
    public int ChainCount
    {
        get { return _chainCount; }
        set { _chainCount = Mathf.Max(0, value); }  // 음수 값 방지
    }

    // 체인 범위 프로퍼티
    public float ChainRadius
    {
        get { return _chainRadius; }
        set { _chainRadius = Mathf.Max(0, value); }  // 음수 값 방지
    }

    // 생성자
    public ChainBehavior(int chainCount, float chainRadius)
    {
        this._chainCount = Mathf.Max(0, chainCount);  // 음수 값 방지
        this._chainRadius = Mathf.Max(0, chainRadius);  // 음수 값 방지
    }


    public void UpdateBehavior(Skill skill)
    {
        if (!initialized)
        {
            Transform target = SkillBehaviorFactory.FindClosestEnemy(skill.transform.position, 100);
            if (target != null)
            {
                // 타겟이 있으면 타겟 방향
                moveDirection = (target.position - skill.transform.position).normalized;
            }
            else
            {
                // 타겟이 없으면 발사되는 방향
                moveDirection = Vector3.forward;
            }
            initialized = true;
        }
        skill.transform.Translate(moveDirection * skill.Speed * Time.deltaTime);
    }
    public void OnHit(Skill skill, Collision collision)
    {
        if (hasHit) return;
        if (!collision.transform.CompareTag("Monster")) return;


        // 체인 타격 대상 탐색
        List<Transform> chainTargets = FindClosestEnemies(skill.transform.position, _chainRadius, _chainCount);

        foreach (var target in chainTargets)
        {
            Monster monster = target.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(skill.Damage);
            }
        }
        DrawChainVisual(skill.transform.position, chainTargets);
        initialized = false;
        skill.ReturnToPool();
    }

    private void DrawChainVisual(Vector3 startPosition, List<Transform> targets)
    {
        GameObject chainObj = new GameObject("ChainVisual");
        lineRenderer = chainObj.AddComponent<LineRenderer>();
        Material lineMat = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
        lineMat.color = new Color(0f, 1f, 1f, 0.8f); // 밝은 시안색 + 약간 투명

        lineRenderer.material = lineMat;
        lineRenderer.startColor = Color.cyan;
        lineRenderer.endColor = Color.white;
        lineRenderer.positionCount = targets.Count + 1;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        lineRenderer.SetPosition(0, startPosition);
        for (int i = 0; i < targets.Count; i++)
        {
            Vector3 targetPos = targets[i].position + Vector3.up * 1.0f;
            lineRenderer.SetPosition(i + 1, targets[i].position);
        }

        GameObject.Destroy(chainObj, 1f); // 시각적 효과 잠깐 유지
    }

    private List<Transform> FindClosestEnemies(Vector3 position, float radius, int count)
    {
        Collider[] hits = Physics.OverlapSphere(position, radius, LayerMask.GetMask("Monster"));
        return hits
            .Where(hit => hit.transform.gameObject.activeInHierarchy)
            .OrderBy(hit => Vector3.Distance(position, hit.transform.position))
            .Take(count)
            .Select(hit => hit.transform)
            .ToList();
    }
}
