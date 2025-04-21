using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int selectedCharacterIndex;
    public Character player;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //�÷��̾� ���� �Ҵ� (�� ���ε� ���)
        if (player == null)
        {
            player = FindObjectOfType<Character>();
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("GameScenes");
    }
    public void RegisterPlayer(Character newPlayer)
    {
        player = newPlayer;
    }

    //�÷��̾ �ı��� �� ȣ�� (�ɼ�)
    public void UnregisterPlayer()
    {
        player = null;
    }
}