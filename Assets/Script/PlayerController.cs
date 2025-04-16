using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference inputActionRef;
    public float moveSpeed;
    public float rotSpeed;
    void Start()
    {
        
    }

    void Update()
    {
        Vector2 dir = inputActionRef.action.ReadValue<Vector2>();
        float translation = Input.GetAxisRaw("Vertical") * moveSpeed;
        float rotation = Input.GetAxisRaw("Horizontal") * rotSpeed;
        
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        transform.Translate(dir * moveSpeed * Time.deltaTime);
        transform.Rotate(0, rotation, 0);
    }
}
