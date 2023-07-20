using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterObject : Monster
{
    [Header("스포너")]
    [SerializeField] private SpawnTrigger trigger;
    [Header("스코어")]

    [SerializeField] private int upScore;


    private void Start()
    {
        score = upScore;
    }

    private void OnDisable()
    {
        if (trigger != null)
        {
            trigger.SpawnMonster();
        }
        PointUp();
        UIManager.instance.ScoreSet();
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
