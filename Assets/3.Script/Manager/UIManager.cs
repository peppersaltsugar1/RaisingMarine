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
    [Header("스킬포인트 텍스트")]
    [SerializeField] Text[] skillPointText;
    [Header("업그레이드 UI")]
    [SerializeField] GameObject[] upgradeUI;
    [Header("슬롯 UI")]
    [SerializeField] GameObject[] slots;
    [Header("인포UI")]
    [SerializeField] GameObject info;
    [Header("ESC버튼")]
    [SerializeField] GameObject escBtn;
    [Header("업그레이드 수치")]
    [SerializeField] Text atkUpgrade;
    [SerializeField] Text defUpgrade;
    [Header("플레이어 HP")]
    [SerializeField] Text playerHp;
    [SerializeField] Image playerPic;

    [Header("플레이어 UI목록")]
    [SerializeField] GameObject[] playerUIList;
    [Header("스킬스프라이트")]
    [SerializeField] Sprite[] skillList;
    [Header("쿨타임UI")]
    [SerializeField] Image[] coolTimeImage;
    [Header("플레이어 HP 스프라이트")]
    [SerializeField] Sprite[] playerHPImageList;


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

    public void MoneySet(int money, int index)
    {
        moneyText[index].text = money.ToString();
    }
    public void SkillPointSet(int skillPoint, int index)
    {
        skillPointText[index].text = skillPoint.ToString();
    }

    public void UseUpgrade()
    {
        ESC();
        for (int i = 0; i < upgradeUI.Length; i++)
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
                if (child.gameObject != slot && child.gameObject.activeSelf&&child.name!="CoolTime")
                {
                    // 활성화된 객체를 비활성화
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
    public void HpSet(int MaxHp, int currentHp)
    {
        if (currentHp > MaxHp * 0.5)
        {
            playerPic.sprite = playerHPImageList[0];
            playerHp.color = Color.green;
        }
        if(currentHp <= MaxHp * 0.5)
        {
            playerPic.sprite = playerHPImageList[1];
            playerHp.color = Color.yellow;
        }
        if(currentHp <= MaxHp * 0.2)
        {
            playerPic.sprite = playerHPImageList[2];
            playerHp.color = Color.red;
        }

        playerHp.text = currentHp.ToString() + "/" + MaxHp.ToString();
    }

    public void PlayerUISet()
    {
        ESC();
        SetInfo();
        for (int i = 0; i < playerUIList.Length; i++)
        {
            playerUIList[i].SetActive(true);
        }
    }
    public void SetInfo()
    {
        info.SetActive(false);
    }

    public void SetUpgradePower(int atk, int def)
    {
        atkUpgrade.text = atk.ToString();
        defUpgrade.text = def.ToString();
    }

    public void BuyHeal()
    {
        Image healImage = playerUIList[3].GetComponent<Image>();
        healImage.sprite = skillList[0];

    }
    public void BuyUlt()
    {
        Image healImage = playerUIList[4].GetComponent<Image>();
        healImage.sprite = skillList[1];
    }
    public void BuyReturn()
    {
        Image healImage = playerUIList[5].GetComponent<Image>();
        healImage.sprite = skillList[2];
    }
    public void BuySteam()
    {
        Image healImage = playerUIList[6].GetComponent<Image>();
        healImage.sprite = skillList[3];    
    }
    public void BuyTeleport()
    {
        Image healImage = playerUIList[7].GetComponent<Image>();
        healImage.sprite = skillList[4];
    }
    public void BuySkillAtk()
    {
        Image healImage = playerUIList[8].GetComponent<Image>();
        healImage.sprite = skillList[5];
    }

    public void StartCool(float cooltime, int index)
    {
        StartCoroutine(CoolTimeRoutine(cooltime,index));
    }
    IEnumerator CoolTimeRoutine(float cooltime,int index)
    {
        coolTimeImage[index].gameObject.SetActive(true);
        var time = cooltime;
        while (true)
        {
            time -= Time.deltaTime;
            var per = time / cooltime;
            coolTimeImage[index].fillAmount = per;
            yield return null;
            if (time <= 0)
            {
                coolTimeImage[index].gameObject.SetActive(false);
                break;
            }
        }

    }


    public void OnSkill()
    {
        ESC();
        escBtn.SetActive(true);
    }


}
