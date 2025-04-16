using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    private PlayerInput playerInput;

    [SerializeField] private GameObject joyStick;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        
    }
    private void Start()
    {

    }    
}