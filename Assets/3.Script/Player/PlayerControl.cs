using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour,ITakeDamage
{
    UnityEvent onDeath;
    [Header("네비메시")]
    [SerializeField]private NavMeshAgent agent;  // 추가

    [Header("애니메이터")]
    [SerializeField] private Animator animator;

    [Header("플레이어 무브관련")]
    [SerializeField] private Camera camera;
    [SerializeField] private bool isMove;
    [SerializeField] private Vector3 destination;


    [Header("플레이어 스탯")]
    [SerializeField] public int playerNum;
    [SerializeField] public int MaxHp;
    [SerializeField] public int Atk;
    [SerializeField] public int Def;
    [SerializeField] public float AtkSpeed;
    [SerializeField] public int AtkRange;
    [SerializeField] public int MoveSpeed;

    [Header("플레이어 파티클")]
    [SerializeField] private ParticleSystem atkParticle;
    [SerializeField] private ParticleSystem gunFireParticle;

    public int currentHp;
    public bool isDead;
    public Transform target;
    [Header("플레이어 공격관련")]
    [SerializeField]private CapsuleCollider playerAtkBox;
    [SerializeField] public float timebetAttack = 0.5f;
    public float lastAttackTimebet;
    private bool canAtk;


    private void Awake()
    {
        camera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        animator = GetComponent<Animator>();
        currentHp = MaxHp;
        canAtk = true;
        playerNum = 1; //이건 나중에 대기방에 들어온 순서대로 번호를 부여해주는걸로 바꿔야함

    }

    void Update()
    {
        if (!isDead)
        {

            if (Input.GetMouseButton(1))
            {
                RaycastHit hit;
                if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit)  )
                {
                    
                    if (hit.collider.gameObject.layer==7)
                    {
                        target = hit.transform;
                    }
                    else
                    {
                        SetDestination(hit.point);
                    }

                }
            }
            LookMoveDirection();
            //if (target != null&& Vector3.Distance(target.position, transform.position) <= AtkRange)
            //{
            //    Attack();
            //}
            if (target != null && canAtk)
            {
                Attack();
            }
            if (agent.velocity.magnitude>0.0f)  // 변경
            {
                isMove = true;
                animator.SetBool("isMove", isMove);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                agent.isStopped = true;
                isMove = false;
                animator.SetBool("isMove", isMove);
                playerAtkBox.enabled = true;
            }

        }
    }

    private void SetDestination(Vector3 dest)
    {
        agent.isStopped = false;
        target = null;
        EndAttack();
        playerAtkBox.enabled = false;
        agent.SetDestination(dest);
        destination = dest;
        isMove = true;
        animator.SetBool("isMove", true);
    }

    // private void Move()
    private void LookMoveDirection()  // 변경
    {
        if (isMove)
        {
            // if (Vector3.Distance(destination, transform.position) <= 0.1f)
            if (agent.velocity.magnitude == 0.0f)  // 변경
            {
                isMove = false;
                animator.SetBool("isMove", isMove);
                playerAtkBox.enabled = true;

                return;
            }

            // var dir = destination - transform.position;
            var dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z) - transform.position;  // 변경
            animator.transform.forward = dir;
        }
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
        Debug.Log(currentHp);
        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
    }
    private void Attack()
    {
        canAtk = false;
        var dir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;  // 변경
        animator.transform.forward = dir;
        animator.SetBool("isMove", false);
        animator.SetBool("isAttack", true);
        gunFireParticle.Play();
        atkParticle.transform.position = target.transform.position;
        atkParticle.Play();

    }

    public void EndAttack()
    {
        gunFireParticle.Stop();
        animator.SetBool("isAttack", false);
        StartCoroutine(atkSpeed_co());

    }
    
    private IEnumerator atkSpeed_co()
    {
        yield return new WaitForSeconds(AtkSpeed);
        canAtk = true;
    }

    public void TakeDamage(int damage, int player)
    {

    }

    private void Die()
    {
        StartCoroutine(Die_co());
    }
    private IEnumerator Die_co()
    {
        gunFireParticle.Stop();
        animator.SetTrigger("isDead");
        canAtk = false;
        isDead = true;
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}

