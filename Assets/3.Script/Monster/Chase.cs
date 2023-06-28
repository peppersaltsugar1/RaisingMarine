using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    private MonsterControl monster;
    private void Start()
    {
        monster = GetComponentInParent<MonsterControl>();
    }

    private void OnTriggerEnter(Collider other)
    {
            Debug.Log("플레이어가들어옴");
        if (other.CompareTag("Player"))
        {
            monster.target = other.transform;
        }
    }
}
