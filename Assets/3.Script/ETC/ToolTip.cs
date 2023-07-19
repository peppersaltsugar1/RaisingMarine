using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour
{
    [SerializeField] GameObject tooltip;

    [SerializeField] Text tooltipText;



    public void ShowToolTip(Slot slot)
    {
        tooltip.SetActive(true);
        tooltipText.text = slot.text;
    }

    public void HideToolTip()
    {
        tooltip.SetActive(false);

    }

}
