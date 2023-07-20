using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [TextArea]
    public string text;


    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 movePos = new Vector3(transform.position.x, transform.position.y + 80, transform.position.z);
        UIManager.instance.ShowToolTip(text,movePos);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.HideToolTip();
    }
}
