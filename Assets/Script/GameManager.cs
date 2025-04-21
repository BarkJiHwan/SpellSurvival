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
        //플레이어 동적 할당 (씬 리로드 대비)
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

    //플레이어가 파괴될 때 호출 (옵션)
    public void UnregisterPlayer()
    {
        player = null;
    }
}