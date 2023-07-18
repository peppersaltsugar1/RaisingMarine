using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

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
    [SerializeField] public float MoveSpeed;
    [Header("�÷��̾� ���׷��̵� ��ġ")]
    [SerializeField] public int atkUp;
    [SerializeField] public int defUp;
    [SerializeField] public int hpUp;
    [SerializeField] public int atkSpeedUp;
    [Header("���׷��̵� ���")]
    [Header("���ݷ°��")]
    [SerializeField] private int atkPower;
    [Header("���°��")]
    [SerializeField] private int defkPower;
    [Header("���ݼӵ�������")]
    [SerializeField] private float atkSpeedPower;
    [Header("ü�°��")]
    [SerializeField] private int hpPower;


    [Header("�÷��̾� �ڿ�")]
    [SerializeField] public int money;
    [SerializeField] public int skillPoint;
    [Header("�÷��̾� ��ƼŬ")]
    [SerializeField] private ParticleSystem atkParticle;
    [SerializeField] private ParticleSystem gunFireParticle;

    public bool isDead;
    public List<Monster> targetList = new List<Monster>();
    public Transform target;
    [Header("�÷��̾� ���ݰ���")]
    [SerializeField]private CapsuleCollider playerAtkBox;
    [SerializeField] public float timebetAttack = 0.5f;
    public float lastAttackTimebet; 
    private bool canAtk;

    [Header("�÷��̾� ��ų ��밡�ɿ���")]
    [SerializeField] public bool canHeal;
    [SerializeField] public bool canUlt;
    [SerializeField] public bool canReturn;
    [SerializeField] public bool canSteam;
    [SerializeField] public bool canTeleport;
    [SerializeField] public bool canSkillAtk; 
    [Header("�÷��̾� ��ų ���ſ���")]
    [SerializeField] public bool buyHeal;
    [SerializeField] public bool buyUlt;
    [SerializeField] public bool buyReturn;
    [SerializeField] public bool buySteam;
    [SerializeField] public bool buyTeleport;
    [SerializeField] public bool buySkillAtk;
    [Header("�÷��̾� ��ų ��Ÿ��")]
    [SerializeField] public int healCool;
    [SerializeField] public int returnCool;
    [SerializeField] public int steamCool;
    [SerializeField] public int ultCool;
    [SerializeField] public int telCool;
    [SerializeField] public int skillAtkCool;
    [Header("�÷��̾� ��ȯ��ġ")]
    [SerializeField] Transform returnPoint;


    private void Awake()
    {
        camera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        animator = GetComponent<Animator>();
        currentHp = MaxHp;
        canAtk = true;
        playerNum = 1; //�̰� ���߿� ���濡 ���� ������� ��ȣ�� �ο����ִ°ɷ� �ٲ����
        UIManager.instance.HpSet(MaxHp,currentHp);

    }

    void Update()
    {
        if (!isDead)
        {
            if (Input.GetMouseButton(1))
            {
                RaycastHit hit;
                if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    if (hit.collider.gameObject.CompareTag("Enemy"))
                    {
                        target = hit.transform;
                        OnTarget(target);
                    }
                    else
                    {
                        SetDestination(hit.point);
                    }
                }
                LookMoveDirection();
            }
            //PlayerMove();
            //LookMoveDirection();
            SetTarget();
            AttackCheck();
            MoveCheck();
            Stop();
            //LookForward();
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
        if (agent.velocity.magnitude == 0.0f)  // ����
        {
            isMove = false;
            animator.SetBool("isMove", isMove);
            playerAtkBox.enabled = true;
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

                Debug.Log(hit.collider.gameObject.tag);
                
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
    private void OnTarget(Transform target)
    {
        agent.isStopped = false;
        EndAttack();
        playerAtkBox.enabled = false;
        agent.SetDestination(target.transform.position);
        destination = target.transform.position;
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
        UIManager.instance.HpSet(MaxHp,currentHp);
    }
    private void Attack()
    {
        canAtk = false;
        agent.isStopped = true;
        //var dir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;  // ����
        //animator.transform.forward = dir;
        animator.SetBool("isMove", false);
        playerAtkBox.enabled = true;
        transform.LookAt(target);
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
        if (target != null)
        {
            target.TryGetComponent(out Monster monster);
            if (monster.isdead)
            {
                target = null;
            }
        }
        else
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
            else if (target != null && targetList.Count <= 0)
            {

                target = null;
            }
        }
        //else
        //{
        //    target = null;
        //}
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
        target = null;
        if (targetList.Count > 0)
        {
            targetList.Clear();
        }
    }

    public void Heal()
    {
        if (!buyHeal)
        {
            if (skillPoint - GameManager.instance.healValue >=0)
            {
                skillPoint -= GameManager.instance.healValue;
                UIManager.instance.SkillPointSet();
                buyHeal = true;
                canHeal = true;
                UIManager.instance.BuyHeal();
                return;
            }
        }
        if (canHeal)
        {
            StartCoroutine(Heal_co());
            UIManager.instance.HpSet(MaxHp,currentHp);
            UIManager.instance.StartCool(healCool,0);
        }
    }

    private IEnumerator Heal_co()
    {
        canHeal = false;
        currentHp = MaxHp;
        yield return new WaitForSeconds(healCool);
        canHeal = true;
    }

    public void Return()
    {
        if (!buyReturn)
        {
            if (skillPoint - GameManager.instance.returnValue >= 0)
            {
                skillPoint -= GameManager.instance.returnValue;
                UIManager.instance.SkillPointSet();
                buyReturn = true;
                canReturn = true;
                UIManager.instance.BuyReturn();
                return;
            }
        }
        if (canReturn)
        {
            StartCoroutine(Return_co());
            UIManager.instance.StartCool(returnCool, 2);
        }
    }

    private IEnumerator Return_co()
    {
        canReturn = false;
        transform.position = returnPoint.position;
        yield return new WaitForSeconds(returnCool);
        canReturn = true;
    }

    public void Ult()
    {
        if (!buyUlt)
        {
            if (skillPoint - GameManager.instance.ultValue >= 0)
            {
                skillPoint -= GameManager.instance.ultValue;
                UIManager.instance.SkillPointSet();
                buyUlt = true;
                canUlt = true;
                UIManager.instance.BuyUlt();
                return;
            }
        }
        if (canUlt)
        {
            StartCoroutine(Ult_co());
            UIManager.instance.StartCool(ultCool, 1);
        }
    }
    
    IEnumerator Ult_co()
    {
        canUlt = false;
        yield return new WaitForSeconds(ultCool);
        canUlt = true;
    }

    public void Steam()
    {
        if (!buySteam)
        {
            if (skillPoint - GameManager.instance.steamValue >= 0)
            {
                skillPoint -= GameManager.instance.steamValue;
                UIManager.instance.SkillPointSet();
                buySteam = true;
                canSteam = true;
                UIManager.instance.BuySteam();
                return;
            }
        }
        if (canSteam)
        {
            StartCoroutine(Steampack_co());
            UIManager.instance.StartCool(steamCool, 3);
        }
    }

    private IEnumerator Steampack_co()
    {
        canSteam = false;
        currentHp -= 10;
        UIManager.instance.HpSet(MaxHp, currentHp);
        AtkSpeed *= 0.5f;
        MoveSpeed *= 1.5f;
        agent.speed = MoveSpeed;
        yield return new WaitForSeconds(20f);
        AtkSpeed *= 2f;
        MoveSpeed /= 1.5f;
        agent.speed = MoveSpeed;
        canSteam = true;
    }

    public void TelePort()
    {
        if (!buyTeleport)
        {
            if (skillPoint - GameManager.instance.TelValue >= 0)
            {
                skillPoint -= GameManager.instance.TelValue;
                UIManager.instance.SkillPointSet();
                buyTeleport = true;
                canTeleport = true;
                UIManager.instance.BuyTeleport();
                return;
            }
        }
        if (canTeleport)
        {
            StartCoroutine(Steampack_co());
            UIManager.instance.StartCool(steamCool, 3);
        }
       
    }

    private IEnumerator Teleport_co()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                transform.position = hit.transform.position;
            }
            yield return new WaitForSeconds(telCool);
        }
    }
}



