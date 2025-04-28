using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterData characterData;

    public float moveSpeed;
    public float rotSpeed;
    public int damage;
    public int playerHp;
    
    float cooldown = 3f;
    void Start()
    {
        moveSpeed = 5f;
        rotSpeed = moveSpeed;
        damage = 100;
        playerHp = 100;
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
            Monster monster = collider.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
            }
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
            playerHp -= dam;
            if (playerHp <= 0)
            {
                playerHp = 0;
                Die();
            }
        }
    }

    public void Die()
    {
        Debug.Log("죽었습니다 ㅋ");
        GameManager.Instance.GameOver();
    }
}
