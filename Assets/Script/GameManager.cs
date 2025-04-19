using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerController player;

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
        {
            player = FindObjectOfType<PlayerController>();
        }
    }
    public void RegisterPlayer(PlayerController newPlayer)
    {
        player = newPlayer;        
    }

    // 플레이어가 파괴될 때 호출 (옵션)
    public void UnregisterPlayer()
    {
        player = null;
    }
}