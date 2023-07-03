using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterControl : Monster
{
    public LayerMask whatIsTarget; // ���� ��� ���̾�

    [Header("���� ������")]
    [SerializeField] MonsterData monsterdata;
    

    public ParticleSystem hitEffect; // �ǰݽ� ����� ��ƼŬ ȿ��
    public AudioClip deathSound; // ����� ����� �Ҹ�
    public AudioClip hitSound; // �ǰݽ� ����� �Ҹ�




    private bool hasTarget
    {
        get
        {
            // ������ ����� �����ϸ� true
            if (target != null)
            {
                return true;
            }

            // �׷��� �ʴٸ� false
            return false;
        }
    }

    private void Start()
    {
        SetData(monsterdata);
        currentHp = MaxHp;
    }

    private void SetData(MonsterData monsterdata)
    {
        MaxHp = monsterdata.MaxHp;
        Atk = monsterdata.Atk;
        Def = monsterdata.Def;
        AtkRange = monsterdata.AtkRange;
        AtkSpeed = monsterdata.AtkSpeed;
        MoveSpeed = monsterdata.MoveSpeed;
        agent.speed = MoveSpeed;
        score = monsterdata.score;
    }
    private void Update()
    {
        if (hasTarget&&!isdead)
        {
            if (Vector3.Distance(target.position, transform.position) <= AtkRange)
            {
                Attack();
            }
            else
            {
                // ���� ��� ���� : ��θ� �����ϰ� AI �̵��� ��� ����
                agent.isStopped = false;
                agent.SetDestination(target.position);
                enemyAnimator.SetBool("isMove", true);
            }

        }
        else
        {
            // ���� ��� ���� : AI �̵� ����
            agent.isStopped = true;
            enemyAnimator.SetBool("isMove", false);

        }

    }



   
}