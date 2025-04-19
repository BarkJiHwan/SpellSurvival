using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private PlayerController player;
    private int currentScore;

    void Awake()
    {
        //�̱���
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        //�÷��̾� ���� �Ҵ� (�� ���ε� ���)
        if (player == null)
            player = FindObjectOfType<PlayerController>();
    }
    public PlayerController GetPlayer() => player;
}