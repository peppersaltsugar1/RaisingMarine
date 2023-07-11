using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRangeAttack : MonoBehaviour
{

    private void OnParticleCollision(GameObject other)
    {
        MonsterControl monster = GetComponentInParent<MonsterControl>();
        if (other.CompareTag("Player"))
        {
            if (!monster.isdead && Time.time >= monster.lastAttackTimebet + monster.timebetAttack)
            {
                monster.lastAttackTimebet = Time.time;
                other.TryGetComponent(out PlayerControl player);
                player.TakeDamage(monster.Atk);
            }
        }
    }
}
