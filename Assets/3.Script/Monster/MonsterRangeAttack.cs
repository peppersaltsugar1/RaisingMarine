using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidBossBreath : MonoBehaviour
{

    private void OnParticleCollision(GameObject other)
    {
        MidBossControl monster = GetComponentInParent<MidBossControl>();
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
