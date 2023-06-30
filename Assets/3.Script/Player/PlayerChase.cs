using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChase : MonoBehaviour
{
    PlayerControl player;
    private void Start()
    {
        player = GetComponentInParent<PlayerControl>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy")&&player.target ==null )
        {
        Debug.Log("¹üÀ§µé¾î¿È");
            player.target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && player.target != null)
        {
            player.target = null;
        }
    }
}
