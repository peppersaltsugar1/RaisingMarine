using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public int playerNum;
    public int[] score;
    [SerializeField] public MonsterSpawner[] spawnerList;
    private void Awake()
    {
        playerNum = 1; //�÷��̾� ������� ���� �ٲ������
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
