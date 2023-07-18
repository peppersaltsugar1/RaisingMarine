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
            // Ʈ���� �ȿ� �÷��̾ �ִ��� Ȯ��
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
