using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exchange : MonoBehaviour
{


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.TryGetComponent(out PlayerControl player);
            //GameManager.instance.score[player.playerNum] 
        }
    }




}
