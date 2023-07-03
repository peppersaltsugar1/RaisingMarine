using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
   
    private void OnParticleCollision(GameObject other)
    {
       PlayerControl player = GetComponentInParent<PlayerControl>();
        if(other.CompareTag("Enemy"))
        {
            if (!player.isDead && Time.time >= player.lastAttackTimebet + player.timebetAttack)
            {
                player.lastAttackTimebet = Time.time;
                other.TryGetComponent(out MonsterControl monster);
                monster.TakeDamage(player.Atk,player.playerNum);
            }
        }
    }
}
