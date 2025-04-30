using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Skill : MonoBehaviour
{
    [Header("스킬 이팩트 프리팹 꽂기")]
    public GameObject effect;

    private ISkillBehavior behavior;
    private float lifeTimer = 0f;

    [HideInInspector]
    public SkillBaseData skillData;

    [Header("스크립터블 오브젝트에서 동적할당 됨 수치 부여X")]
    // 실제 사용 변수들
    public int damage;
    public float speed;
    public float lifeTime;

    public void SetBehavior(ISkillBehavior newBehavior)
    {
        behavior = newBehavior;
    }

    //스킬 베이스 데이터의 값을 수정하지 않고 증가 시키기위해 사용
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
        // 태그가 "Monster"인 객체와 충돌 시
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

            // 스킬의 behavior에 따라 처리
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
