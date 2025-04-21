using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameInitializer : MonoBehaviour
{
    public CharacterDataList characterDataList;
    public Transform spawnPoint;
    public CinemachineVirtualCamera virtualCamera;
    private void Awake()
    {
        int index = GameManager.Instance.selectedCharacterIndex;
        CharacterData data = characterDataList.characters[index];
        GameObject playerChacracter = Instantiate(data.characterPrefab, spawnPoint.position, Quaternion.identity);

        virtualCamera.Follow = playerChacracter.transform;
    }
}
