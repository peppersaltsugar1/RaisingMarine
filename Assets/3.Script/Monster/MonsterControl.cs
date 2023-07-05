using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterControl : Monster
{
    public LayerMask whatIsTarget; // 추적 대상 레이어

    [Header("몬스터 데이터")]
    [SerializeField] MonsterData monsterdata;
    

    public ParticleSystem hitEffect; // 피격시 재생할 파티클 효과
    public AudioClip deathSound; // 사망시 재생할 소리
    public AudioClip hitSound; // 피격시 재생할 소리

    [SerializeField] ParticleSystem atk;
    [SerializeField] ParticleSystem shoot;




    private bool hasTarget
    {
        get
        {
            // 추적할 대상이 존재하면 true
            if (target != null)
            {
                return true;
            }

            // 그렇지 않다면 false
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
        agent.stoppingDistance = AtkRange;
    }
    private void Update()
    {
        if (hasTarget&&!isdead)
        {
            if (Vector3.Distance(target.position, transform.position) <= AtkRange)
            {
                transform.LookAt(target);
                Attack();
            }
            else
            {
                // 추적 대상 존재 : 경로를 갱신하고 AI 이동을 계속 진행
                agent.isStopped = false;
                agent.SetDestination(target.position);
                enemyAnimator.SetBool("isMove", true);
            }

        }
        else
        {
            // 추적 대상 없음 : AI 이동 중지
            agent.isStopped = true;
            enemyAnimator.SetBool("isMove", false);

        }

    }
    public void RangeAttack()
    {
        if (target != null)
        {
            atk.transform.position = target.position;
            shoot.Play();
            atk.Play();
        }
    }




}
