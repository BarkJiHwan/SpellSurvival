using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Character targetPlayer;
    //private MonsterSpawner spawner;
    MonsterSpawnerTest spawner;
    public MonsterData monsterData;
    public int hp;

    void Start()
    {
        spawner = MonsterSpawnerTest.FindAnyObjectByType<MonsterSpawnerTest>();
        CachePlayer();
    }
    public void SetStats(int currentHp, int currentDamage)
    {
        hp = currentHp;
        monsterData.damage = currentDamage;
    }
    private void CachePlayer()
    {
        if (GameManager.Instance != null)
        {
            targetPlayer = GameManager.Instance.player;
        }
    }
    private void Update()
    {
        if (!gameObject.activeSelf)
        { 
            return; 
        }

        Vector3 targetDir = targetPlayer.transform.position - transform.position;
        targetDir.y = 0;
        transform.position = Vector3.MoveTowards(transform.position,
            targetPlayer.transform.position, monsterData.speed * Time.deltaTime);

        if (targetDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                monsterData.rotationSpeed * Time.deltaTime
            );
        }
    }    

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            targetPlayer.TakeDamage(monsterData.damage);
        }
    }
    public void TakeDamage(int dam)
    {        
        if (hp >= 0)
        {
            hp -= dam;
            if (hp <= 0)
            {
                Die();
            }
        }
    }
    public void Die()
    {
        hp = monsterData.hp;
        spawner.ReturnMonster(gameObject);
    }
}
