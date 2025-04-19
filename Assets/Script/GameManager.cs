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
        //싱글톤
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        //플레이어 동적 할당 (씬 리로드 대비)
        if (player == null)
            player = FindObjectOfType<PlayerController>();
    }
    public PlayerController GetPlayer() => player;
}