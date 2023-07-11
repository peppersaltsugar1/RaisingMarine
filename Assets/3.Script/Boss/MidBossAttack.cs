using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidBossAttack : MonoBehaviour
{
    MidBossControl monster;
    private void Start()
    {
        monster = GetComponentInParent<MidBossControl>();
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        other.TryGetComponent(out PlayerControl player);
    //        player.TakeDamage(monster.Atk);
    //        transform.TryGetComponent(out BoxCollider attack);
    //        attack.enabled = false;
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!monster.isdead && Time.time >= monster.lastAttackTimebet + monster.timebetAttack)
            {
                monster.lastAttackTimebet = Time.time;
                other.TryGetComponent(out PlayerControl player);
                player.TakeDamage(monster.Atk);
                transform.TryGetComponent(out BoxCollider attack);
                attack.enabled = false;
            }
        }
    }
}
