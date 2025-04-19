using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //GameObjeckt에서 TestJoyStick찾아서 FloatingJoystick이름 바꾸기
    //[SerializeField] private FloatingJoystick joystick; //<<모바일 연동 시 활성화하기

    [SerializeField] private TestJoyStick joystick;
    private CharacterController characterController;
    public float moveSpeed;
    public float rotSpeed;
    Vector2 dir;
    Vector3 moveDir;
    public int playerHp = 100000;

    private void Awake()
    {
        GameManager.Instance.RegisterPlayer(this);
    }
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (joystick == null)
        {
            Debug.LogWarning("조이스틱이 할당되지 않았습니다!");
            return;
        }

        //조이스틱 입력값 가져오기 (Vector2)
        Vector2 inputDir = joystick.InputVector;

        //이동 방향 계산 (XZ 평면)
        Vector3 moveDir = new Vector3(inputDir.x, 0f, inputDir.y);

        //대각선 이동 속도 보정
        if (moveDir.magnitude > 1f)
            moveDir.Normalize();

        //캐릭터 이동
        characterController.Move(moveDir * moveSpeed * Time.deltaTime);

        //캐릭터 회전
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotSpeed * Time.deltaTime
            );
        }
    }
    void OnDestroy()
    {
        // 씬 전환 시 파괴되면 등록 해제
        if (GameManager.Instance != null)
            GameManager.Instance.UnregisterPlayer();
    }

    public void TakeDamage(int dam)
    {
        if(playerHp >= 0)
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