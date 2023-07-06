using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour,ITakeDamage
{
    UnityEvent onDeath;
    [Header("�׺�޽�")]
    [SerializeField]private NavMeshAgent agent;  // �߰�

    [Header("�ִϸ�����")]
    [SerializeField] private Animator animator;

    [Header("�÷��̾� �������")]
    [SerializeField] private Camera camera;
    [SerializeField] private bool isMove;
    [SerializeField] private Vector3 destination;


    [Header("�÷��̾� ����")]
    [SerializeField] public int playerNum;
    [SerializeField] public int MaxHp;
    [SerializeField] public int currentHp;
    [SerializeField] public int Atk;
    [SerializeField] public int Def;
    [SerializeField] public float AtkSpeed;
    [SerializeField] public int AtkRange;
    [SerializeField] public int MoveSpeed;

    [Header("�÷��̾� ��ƼŬ")]
    [SerializeField] private ParticleSystem atkParticle;
    [SerializeField] private ParticleSystem gunFireParticle;

    public bool isDead;
    public List<MonsterControl> targetList = new List<MonsterControl>();
    public Transform target;
    [Header("�÷��̾� ���ݰ���")]
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
        playerNum = 1; //�̰� ���߿� ���濡 ���� ������� ��ȣ�� �ο����ִ°ɷ� �ٲ����

    }

    void Update()
    {
        if (!isDead)
        {
            PlayerMove();
            LookMoveDirection();
            SetTarget();
            AttackCheck();
            MoveCheck();
            Stop();
            LookForward();
        }
    }   
    private void AttackCheck()
    {
        if (target != null && canAtk && Vector3.Distance(target.position, transform.position) <= AtkRange)
        {
            Attack();
        }
    }
    private void MoveCheck()
    {
        if (agent.velocity.magnitude > 0.0f)  // ����
        {
            isMove = true;
            animator.SetBool("isMove", isMove);
        }
    }
    private void LookForward()
    {
        if (target != null)
        {
            var dir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
            animator.transform.forward = dir;
        }
        else
        {
            // ���� ������ �ٶ󺸵��� ����
            LookMoveDirection();
        }
    }
    private void PlayerMove()
    {
        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {

                if (hit.collider.gameObject.layer == 7)
                {
                    target = hit.transform;
                }
                else
                {
                    SetDestination(hit.point);
                }

            }
        }
    }

    private void SetDestination(Vector3 dest)
    {
        agent.isStopped = false;
        TargetReset();
        EndAttack();
        playerAtkBox.enabled = false;
        agent.SetDestination(dest);
        destination = dest;
        isMove = true;
        animator.SetBool("isMove", true);
    }
    // private void Move()
    private void LookMoveDirection()  // ����
    {
        if (isMove)
        {
            // if (Vector3.Distance(destination, transform.position) <= 0.1f)
            if (agent.velocity.magnitude == 0.0f)  // ����
            {
                isMove = false;
                animator.SetBool("isMove", isMove);
                playerAtkBox.enabled = true;

                return;
            }

            // var dir = destination - transform.position;
            var dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z) - transform.position;  // ����
            animator.transform.forward = dir;
        }
    }
    public void TakeDamage(int damage)
    {
        Debug.Log("�ĸ���");
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
    private void Attack()
    {
        canAtk = false;
        //var dir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;  // ����
        //animator.transform.forward = dir;
        transform.LookAt(target);
        animator.SetBool("isMove", false);
        animator.SetBool("isAttack", true);
        gunFireParticle.Play();
        atkParticle.transform.position = target.transform.position;
        atkParticle.Play();

    }

    private void Stop()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            agent.isStopped = true;
            isMove = false;
            animator.SetBool("isMove", isMove);
            playerAtkBox.enabled = true;
        }
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

    private void SetTarget()
    {
        if (targetList.Count > 0)
        {
            int i = 0;
            while (i < targetList.Count)
            {
                if (targetList[i].isdead)
                {
                    targetList.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            if (targetList.Count > 0)
            {
                target = targetList[0].transform;
            }
            else
            {
                target = null;
            }
        }
        else
        {
            target = null;
        }
        //if (targetList.Count > 0)
        //{
        //    for(int i = 0; i < targetList.Count; i++)
        //    {
        //        if (targetList[i].isdead)
        //        {
        //            targetList.RemoveAt(i);
        //            i -= 1;
        //        }
        //    }
        //    target = targetList[0].transform;
        //}
    }
    private void TargetReset()
    {
        if (targetList.Count > 0)
        {
            targetList.Clear();
        }
    }
}

