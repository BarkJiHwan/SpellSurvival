using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestJoyStick : MonoBehaviour
{
    [Header("조이스틱 UI")]
    public RectTransform background; // 조이스틱 배경
    public RectTransform handle;     // 조이스틱 핸들

    [Header("조이스틱 반경")]
    public float joystickRadius = 100f; // 배경 이미지의 절반 크기(px)

    private int joystickFingerId = -1;
    private Vector2 inputVector = Vector2.zero;

    public Vector2 InputVector => inputVector; // 외부에서 입력값 읽기
    private bool isJoystickActive = false;

    private void Start()
    {
        HideJoystick();
    }

    private void Update()
    {
        // 마우스 클릭(다운) 시 조이스틱 활성화 및 위치 이동
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            ShowJoystick(mousePos);
            isJoystickActive = true;
        }
        // 마우스 버튼이 눌려있는 동안 조이스틱 조작
        else if (Input.GetMouseButton(0) && isJoystickActive)
        {
            Vector2 mousePos = Input.mousePosition;
            UpdateJoystick(mousePos);
        }
        // 마우스 버튼 업 시 조이스틱 비활성화
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
            null, // Camera.main (월드캔버스라면)
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
            null, // Camera.main (월드캔버스라면)
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