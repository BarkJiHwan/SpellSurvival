using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Skill : MonoBehaviour
{
    private ISkillBehavior behavior;
    private float lifeTimer = 0f;

    [HideInInspector]
    public SkillBaseData skillData;

    // ���� ��� ������
    public int damage;
    public float speed;
    public float lifeTime;

    private string poolName;
    public void SetBehavior(ISkillBehavior newBehavior)
    {
        behavior = newBehavior;
    }

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
            else
            {
                ReturnToPool();
            }
        }
    }

    public void ReturnToPool()
    {
        lifeTimer = 0f;
        ObjectPooler.Instance.ReturnToPool(gameObject);
    }
}
