using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    //GameObjeckt에서 TestJoyStick찾아서 FloatingJoystick이름 바꾸기
    //[SerializeField] private FloatingJoystick joystick; //<<모바일 연동 시 활성화하기

    [SerializeField] private TestJoyStick joystick;
    private Rigidbody playerRB;
    public Character character;
        
    Vector2 dir;
    Vector3 moveDir;

    private void Awake()
    {
        character = GetComponent<Character>();
        joystick = TestJoyStick.FindObjectOfType<TestJoyStick>();        
    }
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }
   
    void Update()
    {
        if (joystick == null)
        {
            Debug.LogWarning("조이스틱이 할당되지 않았습니다!");
            return;
        }
       
            Vector2 inputVector = joystick.InputVector;
            Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y).normalized;

            if (moveDir != Vector3.zero)
            {
                transform.Translate(moveDir * character.moveSpeed * Time.deltaTime, Space.World);

                // 회전 방향 설정
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, character.rotSpeed * Time.deltaTime);
            }
        
        ////조이스틱 입력값 가져오기 (Vector2)
        //Vector2 inputDir = joystick.InputVector;

        ////이동 방향 계산 (XZ 평면)
        //Vector3 moveDir = new Vector3(inputDir.x, 0f, inputDir.y);

        ////대각선 이동 속도 보정
        //if (moveDir.magnitude > 1f)
        //    moveDir.Normalize();

        //float pos = inputDir.x * inputDir.y;
        ////캐릭터 이동
        //transform.Translate(moveDir * character.moveSpeed * Time.deltaTime);

        ////캐릭터 회전
        //if (moveDir.sqrMagnitude > 0.001f)
        //{
        //    Quaternion targetRot = Quaternion.LookRotation(moveDir);
        //    transform.rotation = Quaternion.Slerp(
        //        transform.rotation,
        //        targetRot,
        //        character.rotSpeed * Time.deltaTime
        //    );
        //}
    }
    void OnDestroy()
    {
        // 씬 전환 시 파괴되면 등록 해제
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UnregisterPlayer();
        }
    }    
}