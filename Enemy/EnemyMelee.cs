using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    public enum enemyStates { patrol, follow, turn, idle, attack, hurt, parried, parry, block };
    public enemyStates currentState;
    private bool isFollowing;

    private Animator anim;
    private Rigidbody2D theRB;

    [Header("Detecting")]
    public GameObject detectingBoundary;
    public GameObject attackBoundary;

    [Header("LookBack")]
    private bool isPlayerBack;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    private int currentPointIndex;
    public float moveSpeed;

    [Header("Idle")]
    public float waitingTime;
    private float idleCounter;

    [Header("Follow")]
    public GameObject detectingPlayerPoint;  // DetectingPlayerPoint 
    private BoxCollider2D m_detectingRageBoxCol; // DetectingPlayerPoint의 boxCollider2d를 받아오기 위한 변수
    private Vector2 detectingArea;  // 받아온 m_detectingRageBoxCol의 size값으로 디텍팅 범위 설정
    private bool isDetectingPlayer;
    public LayerMask whatIsPlayer;
    public float detectingRange;
    private Vector2 detectingRangeCenter;

    public float followSpeed;
    public float timeBetweenNextMove;
    private float nextMoveCounter;

    [Header("Attack")]
    public GameObject HitPoint;  // attackBox의 parried 값 컨트롤 및 받아오기
    public float detectingAttackRange;
    private BoxCollider2D m_attackRangeBoxCol; // attackRangePoint의 boxcollider2d를 받아오기 위한 변수
    private Vector2 attackArea;  // 받아온 m_attackRnageBox의 size값으로 공격범위 설정
    public float attackLeapForce;
    public GameObject attackRangePoint;
    private Vector2 attackRangeCenter;
    private bool inAttackRange;
    public float attackCoolTime;
    private float attackCounter;
    public GameObject attackEffect;

    [Header("Parried")]
    public float parriedTime;
    public float pushedBackForce;

    [Header("Parrying")]
    public GameObject ParryingBox;
    private bool isParrying; // 패링을 하고 있는 도중에는 다른 상태로 넘어가지 못하도록 하기위해
    public GameObject sparkEffect;
    public Transform sparkEffectPoint;
    public float parryingRate;
    private float currentParryingRate;

    [Header("Take Damage")]
    public TakeDamageMelee _takaDamage;

    private void Start()
    {
        anim = GetComponent<Animator>();
        theRB = GetComponent<Rigidbody2D>();

        

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPoints[i].parent = null;
        }

        currentState = enemyStates.patrol;
        idleCounter = waitingTime;

        m_detectingRageBoxCol = detectingPlayerPoint.GetComponent<BoxCollider2D>();
        m_attackRangeBoxCol = attackRangePoint.GetComponent<BoxCollider2D>();

        detectingArea = new Vector2(m_detectingRageBoxCol.size.x * 0.5f, m_detectingRageBoxCol.size.y * 0.5f);
        attackArea = new Vector2(m_attackRangeBoxCol.size.x * .5f, m_attackRangeBoxCol.size.y * 0.5f);

        attackRangeCenter = attackRangePoint.transform.position;
        detectingRangeCenter = detectingPlayerPoint.transform.position;

        currentParryingRate = parryingRate;
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, detectingAttackRange);
    //    Gizmos.DrawWireSphere(transform.position, detectingRange);
    //}

    void Update()
    {
        isDetectingPlayer = detectingBoundary.GetComponent<EnemyDetectingBoundary>().isDetecting;
        inAttackRange = attackBoundary.GetComponent<EnemyDetectingBoundary>().isDetecting;

        RangePositionUpdates();
        //isDetectingPlayer = Physics2D.OverlapArea(detectingRangeCenter + detectingArea, detectingRangeCenter - detectingArea, whatIsPlayer);
        //inAttackRange = Physics2D.OverlapArea(attackRangeCenter + attackArea, attackRangeCenter - attackArea, whatIsPlayer);
        CheckPlayerPosition();
        CheckInAttackRange();  // attack range 안에 있으면 공격

        // HitPoint의 parried 값이 true라면 parry 상태로 전환
        if (HitPoint.GetComponent<EnemymeleeHitBox>().parried)
        {
            currentState = enemyStates.parried;
        }

        switch (currentState)
        {
            
            case enemyStates.patrol:
                isFollowing = false;
                anim.SetBool("Idle", false);
                anim.SetBool("Patrol", true);

                PatrolDirectionChange();

                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPointIndex].position, moveSpeed * Time.deltaTime);
                if (Mathf.Abs(Vector2.Distance(transform.position, patrolPoints[currentPointIndex].position)) <= 1.5f)
                {
                    
                    if (idleCounter <= 0)
                    {
                        if (currentPointIndex + 1 < patrolPoints.Length)
                        {
                            currentPointIndex++;
                        }
                        else
                        {
                            currentPointIndex = 0;
                        }
                        idleCounter = waitingTime;
                    }
                    else
                    {
                        anim.SetBool("Patrol", false);
                        anim.SetBool("Idle", true);
                        idleCounter -= Time.deltaTime;
                    }
                }
                if (isDetectingPlayer)
                {
                    anim.SetBool("Patrol", false);
                    anim.SetBool("Idle", true);
                    nextMoveCounter = timeBetweenNextMove;
                    idleCounter = waitingTime;
                    currentState = enemyStates.idle;
                }
                break;

            case enemyStates.follow:
                isFollowing = true; // Patrol로 가기 위해 Follow 하고 있었음 True
                anim.SetBool("Idle", false);
                anim.SetBool("Patrol", true);

                if (!isPlayerBack) // 플레이어가 뒤에 있지 않다면 Follow하기
                {
                    if (isDetectingPlayer) // 플레이어를 Detecting한 상태라면 따라다니기
                    {
                        transform.position = Vector2.MoveTowards(transform.position, PlayerController.instance.transform.position, followSpeed * Time.deltaTime);
                    }
                    else // 플레이어를 Detecting 하지 못하면
                    {
                        currentState = enemyStates.idle;         // Idle State로
                        nextMoveCounter = timeBetweenNextMove;   // Idle에 머무를 시간을 초기화
                    }
                }
                else  // 플레이어가 뒤에 있다면 Turn 애니메이션 실행
                {
                    //anim.SetTrigger("Turn"); // Turn 애니메이션은 Loop이 아니기 때문에 재생이 끝나면 다시 Follow를 시작한다
                    //currentState = enemyStates.turn;
                    LookBack();
                }
                break;

            case enemyStates.idle: // Follow하다가 플레이어를 놓치면 idle을 거쳐서 Patrol로, patrol 하다가 플레이어를 감지하면 idle을 거쳐서 follow로
                anim.SetBool("Idle", true);
                anim.SetBool("Patrol", false);
                if (nextMoveCounter > 0f)
                {
                    nextMoveCounter -= Time.deltaTime;
                }
                else
                {
                    anim.SetBool("Idle", false);
                    anim.SetBool("Patrol", true);
                    nextMoveCounter = timeBetweenNextMove;
                    if (!isFollowing)
                    {
                        currentState = enemyStates.follow;
                    }
                    else
                    {
                        currentState = enemyStates.patrol;
                    }
                }
                break;

            case enemyStates.attack:
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("GoulFighter_Attack"))
                {
                    currentState = enemyStates.follow;
                }
                else
                {
                    currentState = enemyStates.attack;
                }
                break;

            case enemyStates.parried:
                anim.SetTrigger("Parried");
                StartCoroutine(Parried());
                currentState = enemyStates.follow;
                break;
        }
    }
    void RangePositionUpdates()
    {
        attackRangeCenter = attackRangePoint.transform.position;
        detectingRangeCenter = detectingPlayerPoint.transform.position;
    }
    void CheckPlayerPosition()
    {
        if (currentState != enemyStates.turn)
        {
            if (transform.position.x > PlayerController.instance.transform.position.x)
            {
                if (transform.eulerAngles.y == 180f)
                {
                    isPlayerBack = true;
                }
                else if (transform.eulerAngles.y == 0f)
                {
                    isPlayerBack = false;
                }
            }
            else if (transform.position.x < PlayerController.instance.transform.position.x)
            {
                if (transform.eulerAngles.y == 0f)
                {
                    isPlayerBack = true;
                }
                else if (transform.eulerAngles.y == 180f)
                {
                    isPlayerBack = false;
                }
            }
        }
    }
    void LookBack()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("GoulFighter_Attack"))  // 공격 도중에는 방향이 바뀌지 않도록
        {
            if (transform.eulerAngles.y == 180f)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            else
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            isPlayerBack = false;
        }
    }
    void PatrolDirectionChange()
    {
        if (transform.position.x > patrolPoints[currentPointIndex].position.x)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (transform.position.x < patrolPoints[currentPointIndex].position.x)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }
    IEnumerator Parried()
    {
        HitPoint.gameObject.SetActive(false);
        PushedBack();
        HitPoint.GetComponent<EnemymeleeHitBox>().parried = false;
        yield return new WaitForSeconds(parriedTime);
    }
    private void CheckInAttackRange()
    {
        if (!isParrying)
        {
            if (inAttackRange)
            {
                if (attackCounter <= 0f)
                {
                    if (!isPlayerBack)
                    {
                        currentState = enemyStates.attack;
                        MeleeAttack();
                    }
                    else
                    {
                        currentState = enemyStates.follow;
                    }
                }
                else
                {
                    attackCounter -= Time.deltaTime;
                }
            }
            else
            {
                anim.SetTrigger("Patrol");
            }
        }
    }
    private void MeleeAttack()
    {
        attackCounter = attackCoolTime;
        anim.SetTrigger("Attack");
        AttackLeap();
    }
    //애니메이션 이벤트 처리
    public void OnAttackBox()
    {
        HitPoint.gameObject.SetActive(true);
    }
    public void OffAttackBOx()
    {
        HitPoint.gameObject.SetActive(false);
    }
    void PushedBack()
    {
        if (transform.eulerAngles.y == 0f)
        {
            theRB.AddForce(Vector2.right * pushedBackForce, ForceMode2D.Impulse);
        }
        else if (transform.eulerAngles.y == 180f)
        {
            theRB.AddForce(-1f * Vector2.right * pushedBackForce, ForceMode2D.Impulse);
        }
    }
    //애니메이션 이벤트에서 처리
    public void OnParryingBox()
    {
        ParryingBox.gameObject.SetActive(true);

    }
    public void OffParryingBox()
    {
        ParryingBox.gameObject.SetActive(false);
        isParrying = false; //패링 끝
    }
    void AttackLeap()
    {
        Transform _targetPoint;
        _targetPoint = transform;

        if (transform.eulerAngles.y == 0f)
        {
            theRB.AddForce(-1f * Vector2.right * attackLeapForce, ForceMode2D.Impulse);
        }
        else if (transform.eulerAngles.y == 180f)
        {
            theRB.AddForce(Vector2.right * attackLeapForce, ForceMode2D.Impulse);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentParryingRate < 0)
        {
            currentParryingRate = 0;
        }

        if (collision.CompareTag("PlayerAttackBox"))
        {
            if (!isParrying)
            {
                float _randomParrying = Random.value;
                if (_randomParrying <= currentParryingRate)
                {
                    if (!isPlayerBack)
                    {
                        if (HitPoint.GetComponent<EnemymeleeHitBox>().parried) // parried 가 true면 
                        {
                            isParrying = false;
                            AudioManager.instance.PlaySFX(2);
                            CameraShake.instance.CamShakeA();

                            _takaDamage.EnemyDie();
                        }
                        else
                        {
                            currentParryingRate = currentParryingRate / 3f;  // 패링할 때마다 다음 패링확률이 1/3로 떨어짐. 패링이 너무 여러번 지속되지 않도록
                            isParrying = true; //패링상태
                            CameraShake.instance.CamShakeA();
                            Parry();
                            
                        }
                    }
                    else    // 뒤에서 공격 받았다면 사망처리
                    {
                        isParrying = false;
                        AudioManager.instance.PlaySFX(2);
                        CameraShake.instance.CamShakeA();

                        _takaDamage.EnemyDie();
                    }
                }
                else    // 패링 상태가 아니라면 사망처리
                {
                    isParrying = false;
                    AudioManager.instance.PlaySFX(2);
                    CameraShake.instance.CamShakeA();

                    _takaDamage.EnemyDie();
                }
            }
        }
        if (collision.CompareTag("PlayerCAttackBox"))
        {
            isParrying = false;
            AudioManager.instance.PlaySFX(2);
            CameraShake.instance.CamShakeA();

            _takaDamage.EnemyDie();
        }
    }
    void Parry()
    {
        anim.SetTrigger("Parry");
        Instantiate(sparkEffect, sparkEffectPoint.transform.position, sparkEffectPoint.transform.rotation);
        if (RespawnManager.instance.isPlayerDead == false)
        {
            AudioManager.instance.PlaySFX(5);
            PlayerController.instance.Parried();
        }
    }
}
