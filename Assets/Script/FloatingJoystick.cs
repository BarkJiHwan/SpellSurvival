using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingJoystick : MonoBehaviour
{
    //모바일 연결 시 이걸로 스크립트 바꾸기.
    [Header("조이스틱 UI")]
    public RectTransform background; // 조이스틱 배경
    public RectTransform handle;     // 조이스틱 핸들

    [Header("조이스틱 반경")]
    public float joystickRadius = 100f; // 배경 이미지의 절반 크기(px)

    private int joystickFingerId = -1;
    private Vector2 inputVector = Vector2.zero;

    public Vector2 InputVector => inputVector; // 외부에서 입력값 읽기

    private void Start()
    {
        HideJoystick();
    }

    private void Update()
    {
        // 터치 입력 처리
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.phase == TouchPhase.Began && joystickFingerId == -1)
                {
                    // 터치 시작: 조이스틱 생성
                    joystickFingerId = touch.fingerId;
                    ShowJoystick(touch.position);
                }
                else if (touch.fingerId == joystickFingerId)
                {
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        // 조이스틱 조작
                        UpdateJoystick(touch.position);
                    }
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        // 터치 종료: 조이스틱 숨김
                        HideJoystick();
                        joystickFingerId = -1;
                    }
                }
            }
        }
        else
        {
            // 터치가 없으면 조이스틱 숨김
            HideJoystick();
            joystickFingerId = -1;
        }
    }

    private void ShowJoystick(Vector2 screenPosition)
    {
        // 조이스틱 UI 활성화 및 위치 이동
        background.gameObject.SetActive(true);
        handle.gameObject.SetActive(true);

        // 스크린 좌표 → 캔버스 좌표 변환
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
        // 스크린 좌표 → 캔버스 좌표 변환
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            screenPosition,
            null, // Camera.main (월드캔버스라면)
            out localPoint
        );

        // 반지름 내로 제한
        Vector2 clamped = Vector2.ClampMagnitude(localPoint, joystickRadius);
        handle.anchoredPosition = clamped;

        // -1~1로 정규화된 입력 벡터 계산
        inputVector = clamped / joystickRadius;
    }

    private void HideJoystick()
    {
        background.gameObject.SetActive(false);
        handle.gameObject.SetActive(false);
        inputVector = Vector2.zero;
    }
}