using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterSelect : MonoBehaviour
{
    public CharacterDataList characterDataLists;
    public GameObject[] pos;
    public Button selectBtn;
    public Button gameStartBtn;

    [SerializeField]private int selectedInext;
    void Start()
    {
        selectBtn.interactable = false;
        gameStartBtn.interactable = false;
        gameStartBtn.onClick.AddListener(() => GameManager.Instance.StartGame());

        
        for (int i = 0; i < characterDataLists.characters.Length; i++)
        {
            CharacterData data = characterDataLists.characters[i];
            GameObject preview = Instantiate(data.characterModel, pos[i].transform.position, Quaternion.identity);

            var previewNumber = preview.AddComponent<ClickableCharacter>();
            previewNumber.characterIndex = i;
        }
    }
  
    public void CharacterSelected(int index)
    {
        GameManager.Instance.selectedCharacterIndex = index;
        gameStartBtn.interactable = true;
        //GameManager.Instance.StartGame();
    }
    void Update()
    {
        // 터치 또는 마우스 클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                ClickableCharacter previewNumber = hit.transform.GetComponent<ClickableCharacter>();
                if (previewNumber != null)
                {
                    CharacterSelected(previewNumber.characterIndex);

                }
            }
        }
        // 모바일 터치 지원
        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        for (int i = 0; i < characterModels.Length; i++)
        //        {
        //            if (hit.transform.gameObject == characterModels[i])
        //            {
        //                SelectCharacter(i);
        //                break;
        //            }
        //        }
        //    }
        //}
    }
}
