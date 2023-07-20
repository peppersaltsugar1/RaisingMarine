using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour
{
    [SerializeField] GameObject tooltip;
    [SerializeField] GameObject upgradeTooltip;
    [SerializeField] Text tooltipText;
    [SerializeField] Text upgradeText;



    public void ShowToolTip(string text, Vector3 pos)
    {
        tooltip.SetActive(true);
        tooltipText.text = text;
        tooltip.transform.position = pos;
    }

    public void HideToolTip()
    {
        tooltip.SetActive(false);
        upgradeTooltip.SetActive(false);
    }

    public void ShowUpgradeToolTip(string text,string upgrade, Vector3 pos)
    {
        tooltip.transform.position = pos;
        tooltip.SetActive(true);
        upgradeTooltip.SetActive(true);
        tooltipText.text = text;
        upgradeText.text = upgrade;

    }


}
