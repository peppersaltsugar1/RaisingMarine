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
    [SerializeField] public int MaxHp=200;
    [SerializeField] public int Atk=10;
    [SerializeField] public int Def=0;
    [SerializeField] public int AtkSpeed;
    [SerializeField] public int AtkRange=5;
    [SerializeField] public int MoveSpeed=5;

    [Header("플레이어 파티클")]
    [SerializeField] private ParticleSystem atkParticle;
    [SerializeField] private ParticleSystem gunFireParticle;

    public int currentHp;
    public bool isDead;
    public Transform target;

    [SerializeField]private CapsuleCollider playerAtkBox;


    private void Awake()
    {
        camera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        animator = GetComponent<Animator>();
        currentHp = MaxHp;

    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                SetDestination(hit.point);
            }
        }
        LookMoveDirection();
        //if (target != null&& Vector3.Distance(target.position, transform.position) <= AtkRange)
        //{
        //    Attack();
        //}
        if (target != null)
        {
            Attack();
        }

    }

    private void SetDestination(Vector3 dest)
    {
        target = null;
        EndAttack();
        playerAtkBox.enabled = false;
        agent.SetDestination(dest);
        destination = dest;
        isMove = true;
        animator.SetBool("isMove", isMove);
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
        Debug.Log("쳐맞음");
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
            //Die();
        }
    }
    private void Attack()
    {
        animator.SetBool("isMove", false);
        var dir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;  // 변경
        animator.transform.forward = dir;
        animator.SetBool("isAttack", true);
        gunFireParticle.Play();
        atkParticle.transform.position = target.transform.position;
        atkParticle.Play();

    }

    public void EndAttack()
    {
        animator.SetBool("isAttack", false);
        gunFireParticle.Stop();

    }
}

