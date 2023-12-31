using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChase : MonoBehaviour
{
    PlayerControl player;
    CapsuleCollider chase;

    private void Start()
    {
        player = GetComponentInParent<PlayerControl>();
        chase = GetComponent<CapsuleCollider>();
        chase.radius = player.AtkRange;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out Monster monster))
            {
                if (!monster.isdead)
                {
                    player.targetList.Add(monster);
                }
            }
        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.TryGetComponent(out MonsterControl monster))
    //    {
    //        if (monster.isdead)
    //        {
    //            player.target = null;
    //        }
    //        else if (other.CompareTag("Enemy") && player.target == null)
    //        {
    //            player.target = other.transform;
    //        }
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Enemy") && player.target != null)
        //{
        //    player.target = null;
        //}
        if (other.CompareTag("Enemy"))
        {
            other.TryGetComponent(out Monster monster);
            player.targetList.Remove(monster);
        }

    }
}
