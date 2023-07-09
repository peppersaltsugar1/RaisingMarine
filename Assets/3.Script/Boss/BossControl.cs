using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControl : Monster
{
    public enum State
    {

    } 

    [Header("보스 데이터")]
    [SerializeField] MonsterData monsterData;
    private void Awake()
    {
        SetData(monsterData);
    }




    private void SetData(MonsterData monsterdata)
    {
        MaxHp = monsterdata.MaxHp;
        Atk = monsterdata.Atk;
        Def = monsterdata.Def;
        AtkRange = monsterdata.AtkRange;
        AtkSpeed = monsterdata.AtkSpeed;
        MoveSpeed = monsterdata.MoveSpeed;
        agent.speed = MoveSpeed;
        score = monsterdata.score;
    }
}
