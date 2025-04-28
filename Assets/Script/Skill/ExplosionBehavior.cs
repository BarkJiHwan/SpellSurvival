using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehavior : ISkillBehavior
{
    private bool isExploding = false;
    private float _explosionRadius;  // 폭발 범위 (필드)
    private float _explosionDelay;   // 폭발 지연 시간 (필드)
    private float timer;
    public float ExplosionRadius
    {
        get { return _explosionRadius; }
        set { _explosionRadius = Mathf.Max(0, value); }  // 폭발 범위는 음수일 수 없으므로, 음수 방지
    }

    public float ExplosionDelay
    {
        get { return _explosionDelay; }
        set { _explosionDelay = Mathf.Max(0, value); }  // 폭발 지연 시간도 음수 방지
    }

    public ExplosionBehavior(float explosionRadius, float explosionDelay)
    {
        this._explosionRadius = Mathf.Max(0, explosionRadius);  // 폭발 범위가 음수가 될 수 없도록 처리
        this._explosionDelay = Mathf.Max(0, explosionDelay);    // 폭발 지연 시간도 음수 방지
    }
    public void UpdateBehavior(Skill skill)
    {
        if (isExploding)
        {
            timer += Time.deltaTime;
            if (timer >= ExplosionDelay)
            {
                Explode(skill);
            }
        }
        else
        {
            skill.transform.Translate(Vector3.forward * skill.speed * Time.deltaTime);
        }
    }

    public void OnHit(Skill skill, Collision collision)
    {
        if (!isExploding)
        {
            isExploding = true;
            timer = 0f;
            skill.speed = 0f; // 충돌 시 이동 멈춤
        }
    }

    private void Explode(Skill skill)
    {
        Collider[] hits = Physics.OverlapSphere(skill.transform.position, ExplosionRadius, LayerMask.GetMask("Monster"));

        foreach (var hit in hits)
        {
            var monster = hit.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(skill.damage);
            }
        }

        skill.ReturnToPool();
    }    
}
