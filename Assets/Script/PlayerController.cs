using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference inputActionRef;
    [SerializeField] private CharacterController characterController;
    public float moveSpeed;
    public float rotSpeed;
    Vector2 dir;
    Vector3 moveDir;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        dir = inputActionRef.action.ReadValue<Vector2>();
        moveDir = new Vector3(dir.x, 0, dir.y);
        characterController.Move(moveDir * moveSpeed * Time.deltaTime);        
        if (moveDir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotSpeed * Time.deltaTime);
        }
    }
}
