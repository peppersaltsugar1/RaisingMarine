using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("���ھ� �ؽ�Ʈ")]
    [SerializeField] Text[] scoreText;
    [Header("�Ӵ� �ؽ�Ʈ")]
    [SerializeField] Text[] moneyText;
    [Header("���׷��̵� UI")]
    [SerializeField] GameObject[] upgradeUI;
    [Header("���� UI")]
    [SerializeField] GameObject[] slots;
    [Header("ESC��ư")]
    [SerializeField] GameObject escBtn;
    [Header("���׷��̵� ��ġ")]
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
            // Slot�� �ڽ� ��ü���� ������
            Transform[] childObjects = slot.GetComponentsInChildren<Transform>();

            // Slot�� �ڽ� ��ü�� �߿��� Ȱ��ȭ�� ��ü�� �ִ��� �˻�
            foreach (Transform child in childObjects)
            {
                if (child.gameObject != slot && child.gameObject.activeSelf)
                {
                    // Ȱ��ȭ�� ��ü�� ��Ȱ��ȭ
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}
