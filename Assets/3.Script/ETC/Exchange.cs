using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exchange : MonoBehaviour
{
    [SerializeField]private int exchange;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ШЏРќСп");
            other.TryGetComponent(out PlayerControl player);
            if (GameManager.instance.score[player.playerNum-1] / exchange > 0)
            {
                int changeMoney = GameManager.instance.score[player.playerNum-1] / exchange;
                int score = GameManager.instance.score[player.playerNum-1] % exchange;
                player.money += changeMoney;
                GameManager.instance.score[player.playerNum-1] = score;
                UIManager.instance.MoneySet(player.money, player.playerNum - 1);
                UIManager.instance.ScoreSet();
            }

        }
    }
        



}
