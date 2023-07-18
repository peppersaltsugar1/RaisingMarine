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
    }

    private void Start()
    {
        spawnerList[0].StartSpawn();
    }

}
