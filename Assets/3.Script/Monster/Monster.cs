using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Monster : MonoBehaviour, ITakeDamage
{
    UnityEvent onDeath;
    public int MaxHp { get; protected set; }
    public int Atk{ get; protected set; }
    public int Def { get; protected set; }
    public int AtkSpeed { get; protected set; }
    public int AtkRange { get; protected set; }
    public int MoveSpeed { get; protected set; }
    public int currentHp;

    public Transform target; // ������ ���
    public NavMeshAgent agent; // ��ΰ�� AI ������Ʈ

    public Animator enemyAnimator; // �ִϸ����� ������Ʈ
    public AudioSource enemyAudioPlayer; // ����� �ҽ� ������Ʈ


    private void Awake()
    {
        //onDeath.AddListener(PointUp);
        agent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
    }
    public void TakeDamage(int damage)
    {
        int takeDamage;
        if (damage - Def <= 0)
        {
            takeDamage = 1;
        }
        else
        {
            takeDamage = (damage - Def);
        }

        currentHp -= takeDamage;
        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
    }

    public void Attack()
    {
        enemyAnimator.SetBool("isMove", false);
        enemyAnimator.SetBool("isAttack", true);
    }

    public void Die()
    {
        if (onDeath != null)
        {
            onDeath.Invoke();
        }
        enemyAnimator.SetBool("isDie", true);

    }

    public void PointUp()
    {

    }

}
