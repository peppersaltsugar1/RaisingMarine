using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillAttack : MonoBehaviour
{
    PlayerControl player;
    int dmg;
    private void Start()
    {
        player = GetComponentInParent<PlayerControl>();
        dmg = player.Atk * 3;   
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            

            if (other.TryGetComponent(out MonsterSpawner spawner))
            {
                    spawner.TakeDamage(dmg);
            }
            if (other.TryGetComponent(out MonsterObject monsterObject))
            {
                    monsterObject.TakeDamage(dmg);
            }
            if (other.TryGetComponent(out MonsterControl monster))
            {
                    monster.TakeDamage(dmg,player.playerNum);
            }
        }
    }

}
