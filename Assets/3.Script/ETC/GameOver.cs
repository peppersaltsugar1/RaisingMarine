using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    
    public void GoTitle()
    {
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlayerBGM("Title");
        AudioManager.instance.PlaySFX("UIClick");
        SceneManager.LoadScene("Title");
    }
}
