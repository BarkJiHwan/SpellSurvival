using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterData characterData;

    public float moveSpeed;
    public float rotSpeed;
    public int damage = 5;
    public int playerHp = 100000;

    void Start()
    {
        moveSpeed = 5f;
        rotSpeed = moveSpeed;
        playerHp = 100000;
        GameManager.Instance.RegisterPlayer(this);
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        float radius = 2f;
        Vector3 attackPos = transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(attackPos, radius);
        foreach (Collider collider in hitColliders) 
        {
            if (collider.CompareTag("Monster"))
            {
                Monster monster = collider.GetComponent<Monster>();
                if (monster != null)
                {
                    Debug.Log( monster.name + "때림 ");
                    monster.TakeDagame(damage);
                }
            }
        }
    }
    public void TakeDamage(int dam)
    {
        if (playerHp >= 0)
        {
            Debug.Log("피 다는중...");
            playerHp -= dam;
        }
        else
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("죽었습니다 ㅋ");
    }
}
