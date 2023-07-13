using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text[] scoreText;

    public static UIManager instance = null;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }



    public void ScoreSet()
    {
        for (int i = 0; i < scoreText.Length; i++)
        {
            scoreText[i].text = $"Score : {GameManager.instance.score[i]}";
        }
    }
}
