using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private GameObject interaction_ob;
    [SerializeField] GameObject upgrade;


    private void Update()
    {
        if (interaction_ob.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                UIManager.instance.UseUpgrade();
                upgrade.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        interaction_ob.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        interaction_ob.SetActive(false);
        upgrade.SetActive(false);
        UIManager.instance.PlayerUISet();
    }
}
