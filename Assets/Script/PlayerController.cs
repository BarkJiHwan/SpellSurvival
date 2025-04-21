using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    //GameObjeckt���� TestJoyStickã�Ƽ� FloatingJoystick�̸� �ٲٱ�
    //[SerializeField] private FloatingJoystick joystick; //<<����� ���� �� Ȱ��ȭ�ϱ�

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
            Debug.LogWarning("���̽�ƽ�� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }
       
            Vector2 inputVector = joystick.InputVector;
            Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y).normalized;

            if (moveDir != Vector3.zero)
            {
                transform.Translate(moveDir * character.moveSpeed * Time.deltaTime, Space.World);

                // ȸ�� ���� ����
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, character.rotSpeed * Time.deltaTime);
            }
        
        ////���̽�ƽ �Է°� �������� (Vector2)
        //Vector2 inputDir = joystick.InputVector;

        ////�̵� ���� ��� (XZ ���)
        //Vector3 moveDir = new Vector3(inputDir.x, 0f, inputDir.y);

        ////�밢�� �̵� �ӵ� ����
        //if (moveDir.magnitude > 1f)
        //    moveDir.Normalize();

        //float pos = inputDir.x * inputDir.y;
        ////ĳ���� �̵�
        //transform.Translate(moveDir * character.moveSpeed * Time.deltaTime);

        ////ĳ���� ȸ��
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
        // �� ��ȯ �� �ı��Ǹ� ��� ����
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UnregisterPlayer();
        }
    }    
}