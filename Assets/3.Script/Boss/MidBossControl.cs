using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MidBossControl : Monster
{
    private bool canAtk;
    private bool canMove;
    private Transform spawn;
    [Header("사용파티클")]
    [SerializeField]ParticleSystem[] useParticle;
    public enum State
    {
        Idle, Attack, Dead, SpinAttack,Breath,Chase,Return
    }

    private State state;
    [Header("보스 데이터")]
    [SerializeField] MonsterData monsterData;

    private void Awake()
    {
        spawn = transform;
        SetData(monsterData);
        currentHp = MaxHp;
        state = State.Idle;
        canAtk = true;
        canMove = true;
    }

    private void OnDisable()
    {
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlayerBGM("Clear");
        SceneManager.LoadScene("Clear");
    }

    private void Update()
    {
        
       SetState();

        switch (state)
        {
            case State.Idle:IdleState();
                break;
            case State.Attack:
                BossAttack();
                break;
            case State.Dead:
                break;
            case State.Chase:
                Chase();
                break;
            case State.Return:
                Return();
                break;
        }
    }

    private void SetState()
    {
        if (isdead)
        {
            state = State.Dead;
            return;
        }
        if(target == null)
        {
            state = State.Idle;
        }
        if(target != null)
        {
            if (Vector3.Distance(target.position, spawn.position) >= 30)
            {
                state = State.Return;
                return;
            }
            if(Vector3.Distance(target.position, transform.position) <= AtkRange&& canAtk)
            {
                state = State.Attack;
                return;
            }
            if(canMove)
            {
                state = State.Chase;
            }
        }
        
    }
    private void BossAttack()
    {
        if (canAtk)
        {

            canAtk = false;
            int atk = Random.Range(0, 3);
            transform.LookAt(target.position);
            switch (atk)
            {
                case 0:
                    enemyAnimator.SetTrigger("isAttack");
                    break;
                case 1:
                    enemyAnimator.SetTrigger("isSpinAttack");
                    break;
                case 2:
                    enemyAnimator.SetTrigger("Breath");
                    break;
            }
        }
    }

    private void SetData(MonsterData monsterdata)
    {
        MaxHp = monsterdata.MaxHp;
        Atk = monsterdata.Atk;
        Def = monsterdata.Def;
        AtkRange = monsterdata.AtkRange;
        AtkSpeed = monsterdata.AtkSpeed;        
        agent.speed = MoveSpeed;
        score = monsterdata.score;
    }
    private void Chase()
    {
        if (canMove)
        {
            //useParticle[0].Play();
            //useParticle[0].Play();
            StartCoroutine(Chase_co());
        }
        else
        {
            state = State.Idle;
        }
    }
    private void Return()
    {
        //useParticle[0].Play();
        StartCoroutine(Return_co());
        //useParticle[0].Play();
    }
    public void BossAtkCool(string atkName)
    {
        StartCoroutine(BossEndAttack_co(atkName));
    }
    private IEnumerator BossEndAttack_co(string atkName)
    {
        enemyAnimator.ResetTrigger(atkName);
        yield return new WaitForSeconds(AtkSpeed);
        canAtk = true;
    }
    private IEnumerator Chase_co()
    {
        canMove = false;
        enemyAnimator.SetBool("isMove", true);
        yield return new WaitForSeconds(5f);
        transform.position = target.position;
        enemyAnimator.SetBool("isMove", false);
    } 
    private IEnumerator Return_co()
    {
        enemyAnimator.SetBool("isMove", true);
        yield return new WaitForSeconds(5f);
        transform.position = spawn.position;
        enemyAnimator.SetBool("isMove", false);
    }
    private void IdleState()
    {
        SetState();
    }
    public void Breath()
    {
        useParticle[1].Play();
    }
}
