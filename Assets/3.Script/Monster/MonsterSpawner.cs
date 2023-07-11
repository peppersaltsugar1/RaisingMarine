using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : Monster
{
    [Header("스폰 몬스터 목록")]
    [SerializeField]MonsterControl[] spawnMonster;
    [SerializeField]MonsterControl monster;
    [SerializeField]MonsterControl boss;
    [SerializeField]private int spawnMonsterNum;
    [Header("스포너 스텟")]
    [SerializeField] private int def;
    [SerializeField] GameObject spawnPoint;

    private void Awake()
    {
        spawnMonster = new MonsterControl[spawnMonsterNum];
        for(int i = 0; i < spawnMonsterNum; i++)
        {
            spawnMonster[i] = Instantiate(monster, transform.position, Quaternion.identity);
            spawnMonster[i].gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        StartCoroutine(Spawn_co());
    }

    private void OnDisable()
    {
        Instantiate(boss, transform.position, Quaternion.identity);
    }



    IEnumerator Spawn_co()
    {
        while (true)
        {
            for (int i = 0; i < spawnMonster.Length; i++)
            {
                if (!spawnMonster[i].gameObject.activeSelf)
                {
                    spawnMonster[i].transform.position = transform.position;
                    spawnMonster[i].gameObject.SetActive(true);
                    spawnMonster[i].target = spawnPoint.transform;
                    //StartCoroutine(SpawnMove_co(i));

                }

                yield return new WaitForSeconds(2f);

                if (i >= spawnMonsterNum-1)
                {
                    i = 0;
                }
            }

        }
    }

    public override void TakeDamage(int damage)
    {
        int dmg = damage - def;
        if (dmg <= 0)
        {
            dmg = 1;
        }
        currentHp -= dmg;
        if (currentHp <= 0)
        {
            transform.gameObject.SetActive(false);
            PointUp();
        }
    }



    private void PointUp()
    {
        for (int i = 0; i < GameManager.instance.playerNum; i++)
        {
            GameManager.instance.score[i] += score;
        }
    }
    //IEnumerator SpawnMove_co(int i)
    //{
    //    float moveSpeed = 1f; // 이동 속도 조절
    //    Vector3 moveDirection = (spawnPoint.transform.position - spawnMonster[i].transform.position).normalized;

    //    while (Vector3.Distance(spawnMonster[i].transform.position, spawnPoint.transform.position) > 0.01f)
    //    {
    //        // 이동한 거리 = 이동 속도 × 경과 시간
    //        float moveDistance = moveSpeed * Time.deltaTime;
    //        spawnMonster[i].transform.position += moveDirection * moveDistance;

    //        yield return null;
    //    }
    //}
}
