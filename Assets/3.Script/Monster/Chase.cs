using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    private MonsterControl monster;
    private void Start()
    {
        monster = GetComponentInParent<MonsterControl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monster.target = other.transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PlayerControl player))
        {
            if (player.isDead)
            {
                monster.target = null;
            }
            if (other.CompareTag("Player")&&!player.isDead)
            {
                monster.target = other.transform;
            }
        }
    }
}
