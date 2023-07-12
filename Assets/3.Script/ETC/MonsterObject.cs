using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterObject : Monster
{
    [Header("½ºÆ÷³Ê")]
    [SerializeField] private SpawnTrigger trigger;
    private void OnDisable()
    {
        if (trigger != null)
        {
            trigger.SpawnMonster();
        }
    }
    public override void TakeDamage(int damage)
    {
        int hitdmg = damage - Def;
        if (hitdmg <= 0)
        {
            hitdmg = 1;
        }
        currentHp -= hitdmg;
        if (currentHp <= 0)
        {
            currentHp = 0;
            transform.gameObject.SetActive(false);
            isdead = true;
            PointUp();
        }
    }

    private void PointUp()
    {
        for (int i = 0; i < GameManager.instance.playerNum; i++)
        {
            GameManager.instance.score[i] += score;
        }
    }


}
