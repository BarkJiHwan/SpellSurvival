using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Skill : MonoBehaviour
{
    [Header("��ų ����Ʈ ������ �ȱ�")]
    public GameObject effect;

    private ISkillBehavior behavior;
    private float lifeTimer = 0f;

    [HideInInspector]
    public SkillBaseData skillData;

    [Header("��ũ���ͺ� ������Ʈ���� �����Ҵ� �� ��ġ �ο�X")]
    // ���� ��� ������
    public int damage;
    public float speed;
    public float lifeTime;

    public void SetBehavior(ISkillBehavior newBehavior)
    {
        behavior = newBehavior;
    }

    //��ų ���̽� �������� ���� �������� �ʰ� ���� ��Ű������ ���
    public void Initialize(SkillBaseData data)
    {        
        skillData = data;
        
        damage = data.baseDamage;
        speed = data.speed;
        lifeTime = data.lifeTime;
    }

    private void Update()
    {
        behavior?.UpdateBehavior(this);

        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
        {
            ReturnToPool();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �±װ� "Monster"�� ��ü�� �浹 ��
        if (collision.gameObject.CompareTag("Monster"))
        {
            Monster monster = collision.gameObject.GetComponent<Monster>();

            if (monster != null)
            {
                GameObject skillEffect = Instantiate(effect);
                skillEffect.transform.position = monster.transform.position;
                Destroy(skillEffect, 3f);
                monster.TakeDamage(damage);
            }

            // ��ų�� behavior�� ���� ó��
            if (behavior is PiercingBehavior piercingBehavior)
            {
                piercingBehavior.OnHit(this, collision);
            }
            else if (behavior is ExplosionBehavior explosionBehavior)
            {
                explosionBehavior.OnHit(this, collision);
            }
            else if (behavior is BoomerangBehavior boomerangBehavior)
            {
            }
            else if (behavior is HomingBehavior homingBehavior)
            {
                homingBehavior.OnHit(this, collision);
            }
            else if( behavior is ChainBehavior chainBehavior)
            {
                chainBehavior.OnHit(this, collision);
            }
        }
    }

    public void ReturnToPool()
    {
        lifeTimer = 0f;
        ObjectPooler.Instance.ReturnToPool(gameObject);
    }
}
