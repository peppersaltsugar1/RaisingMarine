using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRangeAttack : MonoBehaviour
{
    MonsterControl monster;
    [SerializeField]ParticleSystem atk;
    [SerializeField]ParticleSystem shoot;

    private void Awake()
    {
        monster = GetComponentInParent<MonsterControl>();
    }


    public void RangeAttack()
    {
        if (monster.target != null)
        {
            atk.transform.position = monster.target.position;
            shoot.Play();
            atk.Play();
        }
    }
}
