using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptPbj/Character", order = 0)]
public class CharacterData : ScriptableObject
{
    public int characterID;
    public GameObject characterModel;
    public GameObject characterPrefab;
    public Sprite icon;
    //public SkillData skillData;
}