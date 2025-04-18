using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterSpawner Spawner;

    public MonsterData monsterData;
    
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Spawner = MonsterSpawner.FindObjectOfType<MonsterSpawner>();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    transform.position = Vector3.MoveTowards(transform.position, Spawner.player.transform.position, monsterData.speed * Time.deltaTime);
    //}

    void FixedUpdate()
    {
        Vector3 targetPos = Spawner.player.transform.position;
        Vector3 newPos = Vector3.MoveTowards(transform.position, targetPos, monsterData.speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
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
