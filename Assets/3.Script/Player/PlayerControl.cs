using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
    [SerializeField] public int setMaxHp;
    [SerializeField] public int currentHp;
    [SerializeField] public int Atk;
    [SerializeField] public int setAtk;
    [SerializeField] public int Def;
    [SerializeField] public float AtkSpeed;
    [SerializeField] public int AtkRange;
    [SerializeField] public float MoveSpeed;
    [Header("플레이어 업그레이드 수치")]
    [SerializeField] public int atkUp;
    [SerializeField] public int defUp;
    [SerializeField] public int hpUp;
    [SerializeField] public int atkSpeedUp;
    [Header("업그레이드 계수")]
    [Header("공격력계수")]
    [SerializeField] public int atkPower;
    [Header("방어력계수")]
    [SerializeField] public int defkPower;
    [Header("공격속도계수계수")]
    [SerializeField] public float atkSpeedPower;
    [Header("체력계수")]
    [SerializeField] public int hpPower;


    [Header("플레이어 자원")]
    [SerializeField] public int money;
    [SerializeField] public int skillPoint;
    [Header("플레이어 스킬이펙트")]
    [SerializeField] private ParticleSystem atkParticle;
    [SerializeField] private ParticleSystem healParticle;
    [SerializeField] private ParticleSystem gunFireParticle;
    [SerializeField] private ParticleSystem returnParticle;
    [SerializeField] private ParticleSystem telParticle;
    [SerializeField] private ParticleSystem skillatkParticle;

    public bool isDead;
    public List<Monster> targetList = new List<Monster>();
    public Transform target;
    [Header("플레이어 공격관련")]
    [SerializeField]private CapsuleCollider playerAtkBox;
    [SerializeField] public float timebetAttack = 0.5f;
    [SerializeField] public bool canSkill;
    public float lastAttackTimebet; 
    private bool canAtk;

    [Header("플레이어 스킬 사용가능여부")]
    [SerializeField] public bool canHeal;
    [SerializeField] public bool canUlt;
    [SerializeField] public bool canReturn;
    [SerializeField] public bool canSteam;
    [SerializeField] public bool canTeleport;
    [SerializeField] public bool canSkillAtk; 
    [Header("플레이어 스킬 구매여부")]
    [SerializeField] public bool buyHeal;
    [SerializeField] public bool buyUlt;
    [SerializeField] public bool buyReturn;
    [SerializeField] public bool buySteam;
    [SerializeField] public bool buyTeleport;
    [SerializeField] public bool buySkillAtk;
    [Header("플레이어 스킬 쿨타임")]
    [SerializeField] public int healCool;
    [SerializeField] public int returnCool;
    [SerializeField] public int steamCool;
    [SerializeField] public int ultCool;
    [SerializeField] public int telCool;
    [SerializeField] public int skillAtkCool;
    [Header("플레이어 귀환위치")]
    [SerializeField] Transform returnPoint;

    [Header("플레이어 사운드 조건")]
    [SerializeField] private bool canTalk;


    private void Awake()
    {
        camera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        animator = GetComponent<Animator>();
        currentHp = MaxHp;
        canAtk = true;
        playerNum = 1; //이건 나중에 대기방에 들어온 순서대로 번호를 부여해주는걸로 바꿔야함
        UIManager.instance.HpSet(MaxHp,currentHp);
        canSkill = true;
        canTalk = true;
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
            if (UIManager.instance.canchat)
            {
                if (canSkill)
                {
                    if (Input.GetKeyDown(KeyCode.M))
                    {
                        Move_btn();
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        Attack_btn();
                    }
                    if (Input.GetKeyDown(KeyCode.B))
                    {
                        Return();
                    }
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        TelePort();
                    }
                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        Steam();
                    }
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        SkillAtk();
                    }
                    if (Input.GetKeyDown(KeyCode.H))
                    {
                        Heal();
                    }
                }
                if (Input.GetKeyDown(KeyCode.Minus))
                {
                    SHOWMETHEMONEY();
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    UIManager.instance.Chat();
                }
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
        if (agent.velocity.magnitude > 0.0f)  // 변경
        {
            isMove = true;
            animator.SetBool("isMove", isMove);
        }
        if (agent.velocity.magnitude == 0.0f)  // 변경
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
            // 앞쪽 방향을 바라보도록 설정
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
        MoveTalk();
    }
    
    private void AttackMove(Vector3 dest)
    {
        agent.isStopped = false;
        TargetReset();
        EndAttack();
        agent.SetDestination(dest);
        destination = dest;
        isMove = true;
        animator.SetBool("isMove", true);
        MoveTalk();
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
        //var dir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;  // 변경
        //animator.transform.forward = dir;
        animator.SetBool("isMove", false);
        playerAtkBox.enabled = true;
        transform.LookAt(target);
        animator.SetBool("isAttack", true);
        AudioManager.instance.PlaySFX("PlayerAttack");
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
    public void EndSkillAttack()
    {
        gunFireParticle.Stop();
        animator.SetBool("isSkillAttack", false);
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
        AudioManager.instance.PlaySFX("PlayerDie");
        yield return new WaitForSeconds(5f);
        AudioManager.instance.StopBGM();
        AudioManager.instance.PlayerBGM("EndGame");
        SceneManager.LoadScene("GameOver");
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
            if (money - GameManager.instance.healValue >=0)
            {
                money -= GameManager.instance.healValue;
                UIManager.instance.MoneySet(money, playerNum - 1);
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
        healParticle.Play();
        AudioManager.instance.PlaySFX("PlayerHeal");
        canHeal = false;
        currentHp = MaxHp;
        yield return new WaitForSeconds(healCool);
        canHeal = true;
    }

    public void Return()
    {
        if (!buyReturn)
        {
            if (money - GameManager.instance.returnValue >= 0)
            {
                money -= GameManager.instance.returnValue;
                UIManager.instance.MoneySet(money, playerNum-1);
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
        playerAtkBox.enabled = false;
        target = null;
        TargetReset();
        canReturn = false;
        AudioManager.instance.PlaySFX("PlayerReturn");
        returnParticle.Play();
        //SetDestination(returnPoint.position);
        agent.enabled = false;
        transform.position = new Vector3(28, 2, 3);
        camera.transform.position = new Vector3(transform.position.x, camera.transform.position.y, transform.position.z+10);
        playerAtkBox.enabled = true;
        agent.enabled = true;
        yield return new WaitForSeconds(returnCool);
        canReturn = true;
    }

    public void Ult()
    {
        if (!buyUlt)
        {
            if (money - GameManager.instance.ultValue >= 0)
            {
                money -= GameManager.instance.ultValue;
                UIManager.instance.MoneySet(money, playerNum - 1);
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
            if (money - GameManager.instance.steamValue >= 0)
            {
                money -= GameManager.instance.steamValue;
                UIManager.instance.MoneySet(money, playerNum - 1);
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
        AudioManager.instance.PlaySFX("SteamPack");
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
            if (money - GameManager.instance.TelValue >= 0)
            {
                money -= GameManager.instance.TelValue;
                UIManager.instance.MoneySet(money, playerNum - 1);
                buyTeleport = true;
                canTeleport = true;
                UIManager.instance.BuyTeleport();
                return;
            }
        }
        if (canTeleport)
        {
            StartCoroutine(Teleport_co());
        }
       
    }

    private IEnumerator Teleport_co()
    {
        canTeleport = false;
        bool waitingForInput = true;
        canSkill = false;
        while (waitingForInput)
        {
                UIManager.instance.OnSkill();
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    canSkill = true;
                    Debug.Log(hit.transform.position);
                    var dir = new Vector3(hit.point.x, hit.point.y, hit.point.z) - transform.position;  // 변경
                    animator.transform.forward = dir;
                    while (Vector3.Distance(hit.point, transform.position) > 10)
                    {
                        SetDestination(hit.point);
                        LookMoveDirection();
                        yield return null;
                    }
                    //if (Vector3.Distance(hit.transform.position, transform.position) <= 10)
                    //{
                    telParticle.Play();
                    agent.isStopped = true;
                    transform.position = hit.point;
                    UIManager.instance.StartCool(steamCool, 4);
                    waitingForInput = false; // 입력을 받았으므로 대기 상태 해제
                    UIManager.instance.PlayerUISet();
                    //}
                }

              


            }
            canSkill = true;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.instance.PlayerUISet();
                canTeleport = true;
                waitingForInput = true;
                yield break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(telCool);
        canTeleport = true;
        canSkill = true;
    }

    public void SkillAtk()
    {
        if (!buySkillAtk)
        {
            if (money - GameManager.instance.skillAtkValue >= 0)
            {
                money -= GameManager.instance.skillAtkValue;
                UIManager.instance.MoneySet(money, playerNum - 1);
                buySkillAtk = true;
                canSkillAtk = true;
                UIManager.instance.BuySkillAtk();
                return;
            }
        }
        if (canSkillAtk)
        {
            StartCoroutine(SkillAtk_co());
        }
    }

    private IEnumerator SkillAtk_co()
    {
        canSkillAtk = false;
        bool waitingForInput = true;
        canSkill = false;

        while (waitingForInput)
        {
            UIManager.instance.OnSkill();
            if (Input.GetMouseButtonDown(0))
            {
                canSkill = true;
                RaycastHit hit;
                if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    var dir = new Vector3(hit.point.x, hit.point.y, hit.point.z) - transform.position;  // 변경
                    animator.transform.forward = dir;
                    while (Vector3.Distance(hit.point, transform.position) > 10)
                    {
                        agent.SetDestination(hit.point);
                        LookMoveDirection();
                        yield return null;
                    }
                    //if (Vector3.Distance(hit.transform.position, transform.position) <= 10)
                    //{
                    animator.SetBool("isMove", false);
                    agent.isStopped = true;
                    animator.SetBool("isSkillAttack", true);
                    skillatkParticle.gameObject.SetActive(true);
                    AudioManager.instance.PlaySFX("PlayerSkillAttack");
                    skillatkParticle.transform.position = hit.point;
                    skillatkParticle.Play();
                    UIManager.instance.StartCool(skillAtkCool, 5);
                    waitingForInput = false; // 입력을 받았으므로 대기 상태 해제
                    UIManager.instance.PlayerUISet();
                    //}

                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.instance.PlayerUISet();
                canSkillAtk = true;
                waitingForInput = true;
                canSkill = true;

                yield break;
            }

            yield return null;
        }
        canSkill = true;

        yield return new WaitForSeconds(skillAtkCool);
        canSkillAtk = true;
        canSkill = true;

    }
    public void SetUpgrade()
    {
        Atk = (atkUp * atkPower) + setAtk;
        Def = (defUp * defkPower);
        MaxHp = (hpUp * hpPower)+setMaxHp;
        AtkSpeed = (1 - (atkSpeedUp * atkSpeedPower));
    }

    public void Move_btn()
    {
        StartCoroutine(Move_co());
    }
    public void Stop_btn()
    {
        Stop();
    }
    public void Attack_btn()
    {
        StartCoroutine(AttackMove_co());
    }

    private IEnumerator Move_co()
    {
        bool waitingForInput = true;
        canSkill = false;

        while (waitingForInput)
        {
            UIManager.instance.OnSkill();
            if (Input.GetMouseButtonDown(0))
            {
                playerAtkBox.enabled = false;
                canSkill = true;
                RaycastHit hit;
                
                if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    var dir = new Vector3(hit.point.x, hit.point.y, hit.point.z) - transform.position;  // 변경
                    animator.transform.forward = dir;
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
                waitingForInput = false; // 입력을 받았으므로 대기 상태 해제
                UIManager.instance.PlayerUISet();
                //}


            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.instance.PlayerUISet();
                waitingForInput = true;
                canSkill = true;
                yield break;
            }

            yield return null;
        }
        canSkill = true;
        yield return null;

    }
    private IEnumerator AttackMove_co()
    {
        bool waitingForInput = true;
        canSkill = false;

        while (waitingForInput)
        {
            UIManager.instance.OnSkill();
            if (Input.GetMouseButtonDown(0))
            {
                playerAtkBox.enabled = true;
                canSkill = true;
                RaycastHit hit;
                if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    var dir = new Vector3(hit.point.x, hit.point.y, hit.point.z) - transform.position;  // 변경
                    animator.transform.forward = dir;
                    if (hit.collider.gameObject.CompareTag("Enemy"))
                    {
                        target = hit.transform;
                        OnTarget(target);
                    }
                    else
                    {
                        AttackMove(hit.point);
                    }
                }
                waitingForInput = false; // 입력을 받았으므로 대기 상태 해제
                UIManager.instance.PlayerUISet();
                //}


            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.instance.PlayerUISet();
                waitingForInput = true;
                canSkill = true;
                yield break;
            }

            yield return null;
        }
        canSkill = true;
        yield return null;
    }

    private void MoveTalk()
    {
        if (canTalk)
        {
            StartCoroutine(MoveTalk_co());
        }
    }

    private IEnumerator MoveTalk_co()
    {
        canTalk = false;
        int rand = Random.Range(0, 8);
        switch (rand)
        {
            case 0: AudioManager.instance.PlaySFX("MoveTalk1"); break;
            case 1: AudioManager.instance.PlaySFX("MoveTalk2"); break;
            case 2: AudioManager.instance.PlaySFX("MoveTalk3"); break;
            case 3: AudioManager.instance.PlaySFX("MoveTalk4"); break;
            case 4: AudioManager.instance.PlaySFX("MoveTalk5"); break;
            case 5: AudioManager.instance.PlaySFX("MoveTalk6"); break;
            case 6: AudioManager.instance.PlaySFX("MoveTalk7"); break;
            case 7: AudioManager.instance.PlaySFX("MoveTalk8"); break;
        }
        yield return new WaitForSeconds(2f);
        canTalk = true;
    }

    private void SHOWMETHEMONEY()
    {
        money += 10000;
        UIManager.instance.MoneySet(money,playerNum-1);
    }
}



