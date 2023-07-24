using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [Header("강화할 플레이어")]
    [SerializeField]PlayerControl player;
    private int upgradeMoney = 5;

    private void OnEnable()
    {
        player.canSkill = false;
    }
    private void OnDisable()
    {
        player.canSkill = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AtkUpgrade();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            DefUpgrade();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            AtkSpeedUpgrade();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            HpUpgrade();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.instance.PlayerUISet();
        }
    }


    public void AtkUpgrade()
    {
        int money = player.atkUp * upgradeMoney;
        if(money == 0)
        {
            money = upgradeMoney;
        }
        if (player.money - money >= 0)
        {
            player.money -= money;
            player.atkUp++;
            if (player.atkUp == 100)
            {

            }
            UIManager.instance.MoneySet(player.money, player.playerNum-1);
            UIManager.instance.SetUpgradePower(player.atkUp,player.defUp);
            UIManager.instance.UpgradeMoneySet(money.ToString());
            AudioManager.instance.PlaySFX("Upgrade");
            player.SetUpgrade();
        }
        else
        {
            AudioManager.instance.PlaySFX("NotMoney");
        }
        
    }
    public void DefUpgrade()
    {
        int money = player.defUp * upgradeMoney;
        if (money == 0)
        {
            money = upgradeMoney;

        }
        if (player.money - money >= 0)
        {
            player.money -= money;
            player.defUp++;
            UIManager.instance.MoneySet(player.money, player.playerNum - 1);
            UIManager.instance.SetUpgradePower(player.atkUp, player.defUp);
            UIManager.instance.UpgradeMoneySet(money.ToString());
            AudioManager.instance.PlaySFX("Upgrade");

            player.SetUpgrade();
        }
        else
        {
            AudioManager.instance.PlaySFX("NotMoney");
        }
    }
    public void AtkSpeedUpgrade()
    {
        int money = player.atkSpeedUp * upgradeMoney;
        if (money == 0)
        {
            money = upgradeMoney;
        }
        if (player.money - money >= 0)
        {
            player.money -= money;
            player.atkSpeedUp++;
            UIManager.instance.MoneySet(player.money, player.playerNum - 1);
            UIManager.instance.UpgradeMoneySet(money.ToString());
            AudioManager.instance.PlaySFX("Upgrade");

            player.SetUpgrade();
        }
        else
        {
            AudioManager.instance.PlaySFX("NotMoney");
        }
    }
    public void HpUpgrade()
    {
        int money = player.defUp * upgradeMoney;
        if (money == 0)
        {
            money = upgradeMoney;
        }
        if (player.money - money >= 0)
        {
            player.money -= money;
            player.hpUp++;
            UIManager.instance.MoneySet(player.money, player.playerNum - 1);
            player.SetUpgrade();
            UIManager.instance.HpSet(player.MaxHp,player.currentHp);
            UIManager.instance.UpgradeMoneySet(money.ToString());
            AudioManager.instance.PlaySFX("Upgrade");


        }
        else
        {
            AudioManager.instance.PlaySFX("NotMoney");
        }
    }
}
