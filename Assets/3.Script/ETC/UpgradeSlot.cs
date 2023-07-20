using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public enum Type
    {
        ATK,DEF,ATKSPEED,HP
    }

    public Type mytype;
    [TextArea]
    public string text;

    [SerializeField] GameObject upgradeSlot;
    [SerializeField] public Text upgradeMoneyText;
    [SerializeField] PlayerControl player;
    private string money;

   


    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 movePos = new Vector3(transform.position.x, transform.position.y + 80, transform.position.z);
        UIManager.instance.ShowToolTip(text,movePos);
        upgradeSlot.SetActive(true);
        switch (mytype)
        {
            case Type.ATK:
                money = upgradeMoneyText.text = (player.atkUp * 5).ToString();
                UIManager.instance.UpgradeMoneySet(money);
                break;
            case Type.DEF: money = upgradeMoneyText.text = (player.defUp * 5).ToString();
                UIManager.instance.UpgradeMoneySet(money);
                break;
            case Type.ATKSPEED: money = upgradeMoneyText.text = (player.atkSpeedUp * 5).ToString();
                UIManager.instance.UpgradeMoneySet(money);
                break;
            case Type.HP: money = upgradeMoneyText.text = (player.hpUp * 5).ToString();
                UIManager.instance.UpgradeMoneySet(money);
                break;
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.HideToolTip();
        upgradeSlot.SetActive(false);
    }
}
