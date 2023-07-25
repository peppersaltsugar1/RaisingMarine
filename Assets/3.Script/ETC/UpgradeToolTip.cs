using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    public string text;
    [SerializeField] private PlayerControl player;
    [SerializeField] private string type;
    private string upgrade;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(type == "°ø°Ý·Â")
        {
            upgrade = player.setAtk.ToString() +"+"+ (player.atkUp * player.atkPower).ToString();
        }
        else
        {
            upgrade = player.Def.ToString()+"+"+ (player.defUp * player.defkPower).ToString();

        }
        Vector3 movePos = new Vector3(transform.position.x+100, transform.position.y+80, transform.position.z);
        UIManager.instance.ShowUpToolTip(text,upgrade, movePos);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.HideToolTip();
    }
}
