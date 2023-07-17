using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("스코어 텍스트")]
    [SerializeField] Text[] scoreText;
    [Header("머니 텍스트")]
    [SerializeField] Text[] moneyText;
    [Header("업그레이드 UI")]
    [SerializeField] GameObject[] upgradeUI;
    [Header("슬롯 UI")]
    [SerializeField] GameObject[] slots;
    [Header("ESC버튼")]
    [SerializeField] GameObject escBtn;
    [Header("업그레이드 수치")]
    [SerializeField] GameObject upgrade;

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
    
    public void MoneySet(int money,int index)
    {
        moneyText[index].text = money.ToString();
    }
    public void SkllPointSet()
    {

    }

    public void UseUpgrade()
    {
        ESC();
        for(int i = 0; i < upgradeUI.Length; i++)
        {
            upgradeUI[i].gameObject.SetActive(true);
        }
        escBtn.SetActive(true);
    }
    public void ESC()
    {
        foreach (GameObject slot in slots)
        {
            // Slot의 자식 객체들을 가져옴
            Transform[] childObjects = slot.GetComponentsInChildren<Transform>();

            // Slot의 자식 객체들 중에서 활성화된 객체가 있는지 검사
            foreach (Transform child in childObjects)
            {
                if (child.gameObject != slot && child.gameObject.activeSelf)
                {
                    // 활성화된 객체를 비활성화
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}
