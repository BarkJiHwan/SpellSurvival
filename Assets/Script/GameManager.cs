using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int selectedCharacterIndex;
    public Character player;

    public float timer;
    public int stageLevel = 1;
    public bool isStageLevelUp = false;
    public bool isGameStert = false;
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
    private void Update()
    {
        if (isGameStert)
        {
            timer += Time.deltaTime;
            if (timer >= 60 && isStageLevelUp == false)
            {
                isStageLevelUp = true;
            }
            if (isStageLevelUp == true)
            {
                stageLevel++;
                isStageLevelUp = false;
                timer = 0;
            }
        }
    }
    public void StartGame()
    {
        isGameStert=true;
        SceneManager.LoadScene("GameScenes");
    }
    public void GameOver()
    {
        stageLevel = 1;
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