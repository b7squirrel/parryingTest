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
    private BoxCollider2D m_detectingRageBoxCol; // DetectingPlayerPoint�� boxCollider2d�� �޾ƿ��� ���� ����
    private Vector2 detectingArea;  // �޾ƿ� m_detectingRageBoxCol�� size������ ������ ���� ����
    private bool isDetectingPlayer;
    public LayerMask whatIsPlayer;
    public float detectingRange;
    private Vector2 detectingRangeCenter;

    public float followSpeed;
    public float timeBetweenNextMove;
    private float nextMoveCounter;

    [Header("Attack")]
    public GameObject HitPoint;  // attackBox�� parried �� ��Ʈ�� �� �޾ƿ���
    public float detectingAttackRange;
    private BoxCollider2D m_attackRangeBoxCol; // attackRangePoint�� boxcollider2d�� �޾ƿ��� ���� ����
    private Vector2 attackArea;  // �޾ƿ� m_attackRnageBox�� size������ ���ݹ��� ����
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
    private bool isParrying; // �и��� �ϰ� �ִ� ���߿��� �ٸ� ���·� �Ѿ�� ���ϵ��� �ϱ�����
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
        CheckInAttackRange();  // attack range �ȿ� ������ ����

        // HitPoint�� parried ���� true��� parry ���·� ��ȯ
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
                isFollowing = true; // Patrol�� ���� ���� Follow �ϰ� �־��� True
                anim.SetBool("Idle", false);
                anim.SetBool("Patrol", true);

                if (!isPlayerBack) // �÷��̾ �ڿ� ���� �ʴٸ� Follow�ϱ�
                {
                    if (isDetectingPlayer) // �÷��̾ Detecting�� ���¶�� ����ٴϱ�
                    {
                        transform.position = Vector2.MoveTowards(transform.position, PlayerController.instance.transform.position, followSpeed * Time.deltaTime);
                    }
                    else // �÷��̾ Detecting ���� ���ϸ�
                    {
                        currentState = enemyStates.idle;         // Idle State��
                        nextMoveCounter = timeBetweenNextMove;   // Idle�� �ӹ��� �ð��� �ʱ�ȭ
                    }
                }
                else  // �÷��̾ �ڿ� �ִٸ� Turn �ִϸ��̼� ����
                {
                    //anim.SetTrigger("Turn"); // Turn �ִϸ��̼��� Loop�� �ƴϱ� ������ ����� ������ �ٽ� Follow�� �����Ѵ�
                    //currentState = enemyStates.turn;
                    LookBack();
                }
                break;

            case enemyStates.idle: // Follow�ϴٰ� �÷��̾ ��ġ�� idle�� ���ļ� Patrol��, patrol �ϴٰ� �÷��̾ �����ϸ� idle�� ���ļ� follow��
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
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("GoulFighter_Attack"))  // ���� ���߿��� ������ �ٲ��� �ʵ���
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
    //�ִϸ��̼� �̺�Ʈ ó��
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
    //�ִϸ��̼� �̺�Ʈ���� ó��
    public void OnParryingBox()
    {
        ParryingBox.gameObject.SetActive(true);

    }
    public void OffParryingBox()
    {
        ParryingBox.gameObject.SetActive(false);
        isParrying = false; //�и� ��
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
                        if (HitPoint.GetComponent<EnemymeleeHitBox>().parried) // parried �� true�� 
                        {
                            isParrying = false;
                            AudioManager.instance.PlaySFX(2);
                            CameraShake.instance.CamShakeA();

                            _takaDamage.EnemyDie();
                        }
                        else
                        {
                            currentParryingRate = currentParryingRate / 3f;  // �и��� ������ ���� �и�Ȯ���� 1/3�� ������. �и��� �ʹ� ������ ���ӵ��� �ʵ���
                            isParrying = true; //�и�����
                            CameraShake.instance.CamShakeA();
                            Parry();
                            
                        }
                    }
                    else    // �ڿ��� ���� �޾Ҵٸ� ���ó��
                    {
                        isParrying = false;
                        AudioManager.instance.PlaySFX(2);
                        CameraShake.instance.CamShakeA();

                        _takaDamage.EnemyDie();
                    }
                }
                else    // �и� ���°� �ƴ϶�� ���ó��
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
