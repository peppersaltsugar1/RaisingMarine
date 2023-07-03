using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Object/MonsterData", order = int.MaxValue)]
public class MonsterData : ScriptableObject
{
    public int MaxHp;
    public int Atk;
    public int Def;
    public float AtkSpeed;
    public int AtkRange;
    public float MoveSpeed;
    public int score;

}
