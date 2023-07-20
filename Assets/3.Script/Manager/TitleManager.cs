using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    [SerializeField] GameObject tool;
    public void GameStart()
    {
        AudioManager.instance.PlaySFX("UIClick");
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlayerBGM("InGame");
        SceneManager.LoadScene("Ingame");
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void Setting()
    {
        AudioManager.instance.PlaySFX("UIClick");
        tool.SetActive(true);
    }
}
