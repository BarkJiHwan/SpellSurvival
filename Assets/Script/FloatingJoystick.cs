using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingJoystick : MonoBehaviour
{
    //����� ���� �� �̰ɷ� ��ũ��Ʈ �ٲٱ�.
    [Header("���̽�ƽ UI")]
    public RectTransform background; // ���̽�ƽ ���
    public RectTransform handle;     // ���̽�ƽ �ڵ�

    [Header("���̽�ƽ �ݰ�")]
    public float joystickRadius = 100f; // ��� �̹����� ���� ũ��(px)

    private int joystickFingerId = -1;
    private Vector2 inputVector = Vector2.zero;

    public Vector2 InputVector => inputVector; // �ܺο��� �Է°� �б�

    private void Start()
    {
        HideJoystick();
    }

    private void Update()
    {
        // ��ġ �Է� ó��
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.phase == TouchPhase.Began && joystickFingerId == -1)
                {
                    // ��ġ ����: ���̽�ƽ ����
                    joystickFingerId = touch.fingerId;
                    ShowJoystick(touch.position);
                }
                else if (touch.fingerId == joystickFingerId)
                {
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        // ���̽�ƽ ����
                        UpdateJoystick(touch.position);
                    }
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        // ��ġ ����: ���̽�ƽ ����
                        HideJoystick();
                        joystickFingerId = -1;
                    }
                }
            }
        }
        else
        {
            // ��ġ�� ������ ���̽�ƽ ����
            HideJoystick();
            joystickFingerId = -1;
        }
    }

    private void ShowJoystick(Vector2 screenPosition)
    {
        // ���̽�ƽ UI Ȱ��ȭ �� ��ġ �̵�
        background.gameObject.SetActive(true);
        handle.gameObject.SetActive(true);

        // ��ũ�� ��ǥ �� ĵ���� ��ǥ ��ȯ
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
        // ��ũ�� ��ǥ �� ĵ���� ��ǥ ��ȯ
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            screenPosition,
            null, // Camera.main (����ĵ�������)
            out localPoint
        );

        // ������ ���� ����
        Vector2 clamped = Vector2.ClampMagnitude(localPoint, joystickRadius);
        handle.anchoredPosition = clamped;

        // -1~1�� ����ȭ�� �Է� ���� ���
        inputVector = clamped / joystickRadius;
    }

    private void HideJoystick()
    {
        background.gameObject.SetActive(false);
        handle.gameObject.SetActive(false);
        inputVector = Vector2.zero;
    }
}