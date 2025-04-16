using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestJoyStick : MonoBehaviour
{
    [Header("���̽�ƽ UI")]
    public RectTransform background; // ���̽�ƽ ���
    public RectTransform handle;     // ���̽�ƽ �ڵ�

    [Header("���̽�ƽ �ݰ�")]
    public float joystickRadius = 100f; // ��� �̹����� ���� ũ��(px)

    private int joystickFingerId = -1;
    private Vector2 inputVector = Vector2.zero;

    public Vector2 InputVector => inputVector; // �ܺο��� �Է°� �б�
    private bool isJoystickActive = false;

    private void Start()
    {
        HideJoystick();
    }

    private void Update()
    {
        // ���콺 Ŭ��(�ٿ�) �� ���̽�ƽ Ȱ��ȭ �� ��ġ �̵�
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            ShowJoystick(mousePos);
            isJoystickActive = true;
        }
        // ���콺 ��ư�� �����ִ� ���� ���̽�ƽ ����
        else if (Input.GetMouseButton(0) && isJoystickActive)
        {
            Vector2 mousePos = Input.mousePosition;
            UpdateJoystick(mousePos);
        }
        // ���콺 ��ư �� �� ���̽�ƽ ��Ȱ��ȭ
        else if (Input.GetMouseButtonUp(0) && isJoystickActive)
        {
            HideJoystick();
            isJoystickActive = false;
        }
    }

    private void ShowJoystick(Vector2 screenPosition)
    {
        background.gameObject.SetActive(true);
        handle.gameObject.SetActive(true);

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background.parent as RectTransform,
            screenPosition,
            null, // Camera.main (����ĵ�������)
            out localPoint
        );
        background.anchoredPosition = localPoint;
        handle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
    }

    private void UpdateJoystick(Vector2 screenPosition)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            screenPosition,
            null, // Camera.main (����ĵ�������)
            out localPoint
        );
        Vector2 clamped = Vector2.ClampMagnitude(localPoint, joystickRadius);
        handle.anchoredPosition = clamped;
        inputVector = clamped / joystickRadius;
    }

    private void HideJoystick()
    {
        background.gameObject.SetActive(false);
        handle.gameObject.SetActive(false);
        inputVector = Vector2.zero;
    }
}