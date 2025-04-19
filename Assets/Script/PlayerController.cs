using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //GameObjeckt���� TestJoyStickã�Ƽ� FloatingJoystick�̸� �ٲٱ�
    //[SerializeField] private FloatingJoystick joystick; //<<����� ���� �� Ȱ��ȭ�ϱ�

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
            Debug.LogWarning("���̽�ƽ�� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        //���̽�ƽ �Է°� �������� (Vector2)
        Vector2 inputDir = joystick.InputVector;

        //�̵� ���� ��� (XZ ���)
        Vector3 moveDir = new Vector3(inputDir.x, 0f, inputDir.y);

        //�밢�� �̵� �ӵ� ����
        if (moveDir.magnitude > 1f)
            moveDir.Normalize();

        //ĳ���� �̵�
        characterController.Move(moveDir * moveSpeed * Time.deltaTime);

        //ĳ���� ȸ��
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
        // �� ��ȯ �� �ı��Ǹ� ��� ����
        if (GameManager.Instance != null)
            GameManager.Instance.UnregisterPlayer();
    }

    public void TakeDamage(int dam)
    {
        if(playerHp >= 0)
        {
            Debug.Log("�� �ٴ���...");
            playerHp -= dam;
        }
        else
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("�׾����ϴ� ��");
    }
}