using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehavior : ISkillBehavior
{
    private bool isExploding = false;
    private float _explosionRadius;  // ���� ���� (�ʵ�)
    private float _explosionDelay;   // ���� ���� �ð� (�ʵ�)
    private float timer;
    public float ExplosionRadius
    {
        get { return _explosionRadius; }
        set { _explosionRadius = Mathf.Max(0, value); }  // ���� ������ ������ �� �����Ƿ�, ���� ����
    }

    public float ExplosionDelay
    {
        get { return _explosionDelay; }
        set { _explosionDelay = Mathf.Max(0, value); }  // ���� ���� �ð��� ���� ����
    }

    public ExplosionBehavior(float explosionRadius, float explosionDelay)
    {
        this._explosionRadius = Mathf.Max(0, explosionRadius);  // ���� ������ ������ �� �� ������ ó��
        this._explosionDelay = Mathf.Max(0, explosionDelay);    // ���� ���� �ð��� ���� ����
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
            skill.speed = 0f; // �浹 �� �̵� ����
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
