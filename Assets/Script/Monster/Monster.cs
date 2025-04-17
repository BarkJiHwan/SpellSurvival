using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterData _Data;
    protected float _Hp;
    protected float _Speed;
    protected int _Damage;
    protected bool _IsTargeting;
    public virtual void SetupMonster(MonsterData data, float hp, float speed, int damage, bool isTargeting)
    {
        _Data = data;
        _Hp = hp;
        _Speed = speed;
        _Damage = damage;
        _IsTargeting = isTargeting;
    }


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
