using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [TextArea]
    public string text;
    [SerializeField]private PlayerControl player;

    [SerializeField]private ToolTip tooltip;
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowToolTip(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideToolTip();
    }
}
