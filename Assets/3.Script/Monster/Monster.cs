using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Monster : MonoBehaviour, ITakeDamage
{
    UnityEvent onDeath;
    public int MaxHp { get; protected set; }
    public int Atk { get; protected set; }
    public int Def { get; protected set; }
    public float AtkSpeed { get; protected set; }
    public int AtkRange { get; protected set; }
    public float MoveSpeed { get; protected set; }
    public int currentHp;
    private bool canAttack;
    public int score { get; protected set; }

    public bool isdead;

    [SerializeField] public float timebetAttack = 0.5f;
    public float lastAttackTimebet;

    public Transform target; // ������ ���
    public NavMeshAgent agent; // ��ΰ�� AI ������Ʈ

    public Animator enemyAnimator; // �ִϸ����� ������Ʈ
    public AudioSource enemyAudioPlayer; // ����� �ҽ� ������Ʈ


    private void Awake()
    {
        //onDeath.AddListener(PointUp);
        agent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        canAttack = true;
    }
    public void TakeDamage(int damage,int playerNum)
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
            Die(playerNum,score);
        }
    }

    public void Attack()
    {
        if (canAttack)
        {
            //enemyAnimator.SetBool("isMove", false);
            ////enemyAnimator.SetBool("isAttack", true);
            //enemyAnimator.SetTrigger("onAttack");
            StartCoroutine(Attack_co());
        }
    }

    public void Die(int playernum,int score)
    {
        if (onDeath != null)
        {
            onDeath.Invoke();
        }
        PointUp(playernum, score);
        StartCoroutine(Die_co());
    }

    public void PointUp(int playerNum,int score)
    {
        GameManager.instance.score[playerNum - 1] += score;
        Debug.Log(GameManager.instance.score[playerNum - 1]);
    }

    public void EndAttack()
    {
        enemyAnimator.ResetTrigger("onAttack");
    }

    IEnumerator Attack_co()
    {
        canAttack = false;
        enemyAnimator.SetBool("isMove", false);
        enemyAnimator.SetTrigger("onAttack");
        yield return new WaitForSeconds(AtkSpeed);
        canAttack = true;
    }
    IEnumerator Die_co()
    {
        isdead = true;
        enemyAnimator.SetTrigger("isDie");
        agent.isStopped = true;
        yield return new WaitForSeconds(3f);    
        transform.gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
    }
}