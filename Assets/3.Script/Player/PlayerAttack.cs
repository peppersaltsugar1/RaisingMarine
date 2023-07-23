using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
   
    private void OnParticleCollision(GameObject other)
    {
       PlayerControl player = GetComponentInParent<PlayerControl>();
        if (other == null)
        {
            return;
        }
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out MonsterSpawner spawner))
            {
                if (!player.isDead && Time.time >= player.lastAttackTimebet + player.timebetAttack)
                {
                    player.lastAttackTimebet = Time.time;
                    spawner.TakeDamage(player.Atk);
                }
                    return;
            }
            if (other.TryGetComponent(out MonsterObject monsterObject))
            {
                if (!player.isDead && Time.time >= player.lastAttackTimebet + player.timebetAttack)
                {
                    player.lastAttackTimebet = Time.time;
                    monsterObject.TakeDamage(player.Atk);
                }
                return;
            }
            if (other.TryGetComponent(out MonsterControl monster))
            {
                if (!player.isDead && Time.time >= player.lastAttackTimebet + player.timebetAttack)
                {
                    player.lastAttackTimebet = Time.time;
                    monster.TakeDamage(player.Atk, player.playerNum);
                }
            }
        }
    }
}
