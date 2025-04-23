using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Character targetPlayer;
    public MonsterData monsterData;
    private MonsterSpawner spawner; //스폰 위치(부모)
    [Header("레벨당 바뀌는 값 보기 위한 값")]
    [SerializeField]private int hp;

    void Start()
    {
        hp = monsterData.hp; //레벨당 바뀌는 값 보기 편하도록...
        spawner = transform.parent.GetComponent<MonsterSpawner>();
        CachePlayer();
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
