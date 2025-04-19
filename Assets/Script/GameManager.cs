using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerController player;

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
        {
            player = FindObjectOfType<PlayerController>();
        }
    }
    public void RegisterPlayer(PlayerController newPlayer)
    {
        player = newPlayer;        
    }

    // �÷��̾ �ı��� �� ȣ�� (�ɼ�)
    public void UnregisterPlayer()
    {
        player = null;
    }
}