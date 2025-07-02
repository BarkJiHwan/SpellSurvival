using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [Header("스킬 이팩트 프리팹 꽂기")]
    public GameObject _effect;

    private ISkillBehavior _behavior;
    private float _lifeTimer = 0f;

    [HideInInspector]
    public ActiveSkillData _skillData;

    // 실제 사용 변수들
    private int damage;
    private float speed;
    private float lifeTime;

    public int Damage { get => damage; set => damage = value; }
    public float Speed { get => speed; set => speed = value; }
    public float LifeTime { get => lifeTime; set => lifeTime = value; }

    private void Start()
    {
        if(_skillData.skillType == SkillType.Active)
        {
            for (int i = 0; i <= _skillData.maxHits; i++)
            {
                var skillEffect = Instantiate(_effect, transform.parent);
                _effect = skillEffect;
                _effect.SetActive(false);
            }
        }
    }
    public void SetBehavior(ISkillBehavior newBehavior)
    {
        _behavior = newBehavior;
    }

    //스킬 베이스 데이터의 값을 수정하지 않고 증가 시키기위해 사용
    public void Initialize(SkillBaseData data)
    {
        _skillData = (ActiveSkillData)data;
        
        Damage = data.baseDamage;
        Speed = data.speed;
        LifeTime = data.lifeTime;
    }

    private void Update()
    {
        _behavior?.UpdateBehavior(this);

        _lifeTimer += Time.deltaTime;
        if (_lifeTimer >= LifeTime)
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
                _effect.transform.position = monster.transform.position;
                _effect.SetActive(true);
                monster.TakeDamage(Damage);
            }

            // 스킬의 behavior에 따라 처리
            if (_behavior is PiercingBehavior piercingBehavior)
            {
                piercingBehavior.OnHit(this, collision);
            }
            else if (_behavior is ExplosionBehavior explosionBehavior)
            {
                explosionBehavior.OnHit(this, collision);
            }
            else if (_behavior is BoomerangBehavior boomerangBehavior)
            {
            }
            else if (_behavior is HomingBehavior homingBehavior)
            {
                homingBehavior.OnHit(this, collision);
            }
            else if(_behavior is ChainBehavior chainBehavior)
            {
                chainBehavior.OnHit(this, collision);
            }
        }
    }

    public void ReturnToPool()
    {
        _lifeTimer = 0f;
        ObjectPooler.Instance.ReturnToPool(gameObject);
    }
}
