using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private PlayerController targetPlayer;
    private MonsterSpawner spawner;
    public MonsterData monsterData;
    
    void Start()
    {
        spawner = MonsterSpawner.FindAnyObjectByType<MonsterSpawner>();
        CachePlayer();
    }
    private void CachePlayer()
    {
        if (GameManager.Instance != null)
        {
            targetPlayer = GameManager.Instance.player;
        }
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPlayer.transform.position, monsterData.speed * Time.deltaTime);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            targetPlayer.TakeDamage(monsterData.damage);
        }
    }
    public void TakeDagame(int dam)
    {
        if (monsterData.hp >= 0)
        {
            monsterData.hp -= dam;
        }
        else
        {
            Die();
        }
    }
    public void Die()
    {
        spawner.ReturnMonster(gameObject);
    }
}
