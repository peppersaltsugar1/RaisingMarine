using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healling : MonoBehaviour
{
    private List<PlayerControl> playerList;


    private void Start()
    {
        playerList = new List<PlayerControl>();
        StartCoroutine(Heal_co());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.TryGetComponent(out PlayerControl player);
            playerList.Add(player);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.TryGetComponent(out PlayerControl player);
            playerList.Remove(player);
        }
    }

    private IEnumerator Heal_co()
    {


        while (true)
        {
            // 트리거 안에 플레이어가 있는지 확인
            if (playerList.Count>0)
            {
                for(int i = 0; i < playerList.Count; i++)
                {
                    playerList[i].currentHp = playerList[i].MaxHp;
                    UIManager.instance.HpSet(playerList[i].MaxHp, playerList[i].currentHp);
                }
            }
            yield return new WaitForSeconds(3f);
        }

    }
}
