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

    public int currentHp;
    public bool isDead;
    public Transform target;

    private CapsuleCollider playerAtkBox;


    private void Awake()
    {
        camera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        animator = GetComponent<Animator>();
        currentHp = MaxHp;
        playerAtkBox = GetComponentInChildren<CapsuleCollider>();

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
    }

    private void SetDestination(Vector3 dest)
    {
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
}

