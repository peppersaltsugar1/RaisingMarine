using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Monster : MonoBehaviour, ITakeDamage
{
    UnityEvent onDeath;
    public string monsterName;
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

    protected CapsuleCollider hitBox;

    [SerializeField] public float timebetAttack = 0.5f;
    public float lastAttackTimebet;

    public Transform target; // 추적할 대상
    public NavMeshAgent agent; // 경로계산 AI 에이전트

    public Animator enemyAnimator; // 애니메이터 컴포넌트
    public AudioSource enemyAudioPlayer; // 오디오 소스 컴포넌트

    [SerializeField]ParticleSystem explosion; //폭발 파티클


    private void Awake()
    {
        hitBox = GetComponent<CapsuleCollider>();
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
        UIManager.instance.ScoreSet();
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
        AudioManager.instance.PlaySFX(monsterName + "Attack");
        yield return new WaitForSeconds(AtkSpeed);
        canAttack = true;
    }
    IEnumerator Die_co()
    {
        hitBox.enabled = false;
        isdead = true;
        enemyAnimator.SetTrigger("isDie");
        AudioManager.instance.PlaySFX(monsterName + "Die");
        agent.isStopped = true;
        yield return new WaitForSeconds(3f);    
        transform.gameObject.SetActive(false);
    }

    public virtual void TakeDamage(int damage)
    {
    }

    public void Explosion()
    {
        StartCoroutine(Explosion_co());
    }

    IEnumerator Explosion_co()
    {
        explosion.Play();
        agent.isStopped = true;
        
        yield return new WaitForSeconds(0.5f);
        transform.gameObject.SetActive(false);
    }
}
