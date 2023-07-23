using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [Header("�����ȸ��͸��")]
    [SerializeField] private Monster[] spawnMonsterList;
    [Header("��������")]
    [SerializeField]private Monster[] monsterList;
    [Header("��������")]
    [SerializeField] private Transform spawnPoint;

    private void Awake()
    {
        spawnMonsterList = new Monster[monsterList.Length];
       for(int i =0; i < monsterList.Length; i++)
        {
            spawnMonsterList[i] = Instantiate(monsterList[i], transform.position, Quaternion.identity);
            spawnMonsterList[i].gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.PlaySFX("MonsterSpawn");
            SpawnMonster();
        }
    }

    public void SpawnMonster()
    {
        for (int i = 0; i < spawnMonsterList.Length; i++)
        {
            spawnMonsterList[i].gameObject.SetActive(true);
            spawnMonsterList[i].target = spawnPoint;
        }
        transform.gameObject.SetActive(false);
    }
}
