using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public int playerNum;
    public int[] score;
    [SerializeField] public MonsterSpawner[] spawnerList;

    [Header("스킬가격")]
    [SerializeField]public int healValue;
    [SerializeField]public int ultValue;
    [SerializeField]public int returnValue;
    [SerializeField]public int steamValue;
    [SerializeField]public int TelValue;
    [SerializeField]public int skillAtkValue;

    [SerializeField] PlayerControl player;
    [SerializeField] GameObject boss;
    private bool canSpawn;
    private bool canBossSpawn;
    private void Awake()
    {
        playerNum = 1; //플레이어 입장수의 따라서 바꿔줘야함
        score = new int[playerNum];
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }   
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        canBossSpawn = true;
    }
    private void Update()
    {
        for(int i = 0; i < spawnerList.Length; i++)
        {
            canSpawn = false;
            if (spawnerList[i].gameObject.activeSelf)
            {
                canSpawn = true;
                break;
            }
        }
        if (!canSpawn)
        {
            if (canBossSpawn)
            {
                SpawnBoss();
            }
        }
    }
    private void Start()
    {
        spawnerList[0].StartSpawn();
    }

    private void SpawnBoss()
    {
        canBossSpawn = false;
        Instantiate(boss, new Vector3(-160, 0, 180), Quaternion.identity);
    }

    public void SHOWMETHEMONEY()
    {
        player.money += 10000;
        UIManager.instance.MoneySet(10000, playerNum - 1);
    }


    public void PowerOverwhelming()
    {
        player.MaxHp = 99999;
        player.currentHp = 99999;
        player.Def = 255;
        UIManager.instance.HpSet(99999,99999);
        
    }

    public void BlackSheepWall()
    {
        GameObject obj = GameObject.Find("ObjectName");
        if (obj != null)
        {
            obj.SetActive(false);
        }
    }

}
