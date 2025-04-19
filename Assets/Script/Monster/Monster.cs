using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterSpawner Spawner;

    public MonsterData monsterData;
    
    void Start()
    {
        Spawner = MonsterSpawner.FindObjectOfType<MonsterSpawner>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Spawner.player.transform.position, monsterData.speed * Time.deltaTime);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            Spawner.player.TakeDamage(monsterData.damage);
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
        Spawner.ReturnMonster(gameObject);
    }
}
