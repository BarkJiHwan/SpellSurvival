using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterData characterData;

    public float moveSpeed;
    public float rotSpeed;
    public int damage = 5;
    public int playerHp = 100000;
    
    float cooldown = 3f;
    void Start()
    {
        moveSpeed = 5f;
        rotSpeed = moveSpeed;
        playerHp = 100000;
        GameManager.Instance.RegisterPlayer(this);
        StartCoroutine(Attackable());
    }

    void Update()
    {
        
    }
    IEnumerator Attackable()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldown);
            Debug.Log("공격!");
            Attack();
        }
    }
    public void Attack()
    {
        float radius = 10f;
        Vector3 attackPos = transform.position;
        
        Collider[] hitColliders = Physics.OverlapSphere(attackPos, radius, LayerMask.GetMask("Monster"));
        foreach (Collider collider in hitColliders) 
        {
            //if (collider.CompareTag("Monster"))
            //{
                Monster monster = collider.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.TakeDagame(damage);
                }
            //}
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // 기즈모 색상 설정
        Gizmos.DrawWireSphere(transform.position, 10); // 공격 범위를 원으로 표시
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
