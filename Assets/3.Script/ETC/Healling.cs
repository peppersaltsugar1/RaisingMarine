using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healling : MonoBehaviour
{



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TryGetComponent(out PlayerControl player);
            player.currentHp = player.MaxHp;
        }    
    }
}
