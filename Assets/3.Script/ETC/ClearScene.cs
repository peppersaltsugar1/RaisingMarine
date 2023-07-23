using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearScene : MonoBehaviour
{
   
    public void EXIT()
    {
        AudioManager.instance.PlaySFX("UIClick");
        Application.Quit();
    }
}
