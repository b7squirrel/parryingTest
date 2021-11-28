using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public Animator anim;
    public Rigidbody2D theRB;
    public SpriteRenderer theSR;

    [HideInInspector]
    public float direction;  // 0을 포함하지 않는 방향
    [Header("Move")]
    public float moveSpeed;
    private float xDirection;  // 0을 포함하는 방향 input.GetAxisRaw("Horizontal")과 일치. 
    public float stopToMoveForce;
    
    [HideInInspector]
    public bool isGrounded;

    [Header("Ground Check")]
    public LayerMask whatIsGround;
    public Transform groundCheck;

    [Header("Jump")]
    public float jumpForce;
    public float jumpRememberTime;
    private float jumpRemember;

    [Header("Pushed Back")]
    public float pushedBackTime;
    public float pushedBackCounter;
    private float pushedBackForce;

    //dashRoll variables
    [Header("Dash Roll")]
    public float dashRollCounter;
    public float dashsRollLength;
    public float dashRollForce;
    public float timeBetweenDashRoll;
    private float dashRollCoolTime;
    private bool dashRollSound; // dash Roll이 실행되면 한 번만 재생이 되도록

    //parried variables
    [HideInInspector]
    public float parriedBackForce;

    private BoxCollider2D theBox;
    private CircleCollider2D theCircle;

    [Header("light")]
    public GameObject candle;
    public Transform candlePoint;
    private GameObject _candle;

    [Header("Dust")]
    public GameObject dustJump;
    public Transform dustPoint;
    public GameObject dustLand;
    public GameObject dustWalk;
    public float timeBetweenDust;
    private float dustCounter;

    [Header("Attack")]  
    public float attackForce_x;  // attack 할 때 앞으로 돌진하는 힘, y축 점프는 PlayerAttack 스크립트에 있음
        
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        theSR = GetComponent<SpriteRenderer>();
        theBox = GetComponent<BoxCollider2D>();
        theCircle = GetComponent<CircleCollider2D>();

        direction = 1f;
        pushedBackCounter = 0f;

        //_candle = Instantiate(candle, candlePoint.position, Quaternion.identity);
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.up * -1f);
        if (pushedBackCounter <= 0f)
        {
            anim.SetBool("Parried", false);

            if (dashRollCounter <= 0f)
            {
                //  colliders 초기화
                PlayerPlayer();
                dashRollSound = false;

                // Grounded
                isGrounded = Physics2D.OverlapCircle(groundCheck.position, .05f, whatIsGround);

                // Dust Walk 멈추었다 걸을 때, 방향을 바꿀 때만 먼지 생성
                if (direction != Input.GetAxisRaw("Horizontal") && Input.GetAxisRaw("Horizontal") != 0 && isGrounded)
                {
                    DustWalk();
                }

                if (xDirection == 0 && Input.GetAxisRaw("Horizontal") != 0 && isGrounded) // 정지상태였다가 움직인다면 먼지 생성
                {
                    DustWalk();
                }

                // Direction
                if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    direction = Input.GetAxisRaw("Horizontal");
                }

                if (direction > 0f)
                {
                    transform.eulerAngles = new Vector3(0f, 0f, 0f);
                }
                else if (direction < 0f)
                {
                    transform.eulerAngles = new Vector3(0f, 180f, 0f);
                }

                // Move
                xDirection = Input.GetAxisRaw("Horizontal");
                theRB.velocity = new Vector2(moveSpeed * xDirection, theRB.velocity.y);


                // Jump
                jumpRemember -= Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.X))
                {
                    jumpRemember = jumpRememberTime;
                }

                if (isGrounded && jumpRemember > 0)
                {
                    DustJump();
                    theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                    AudioManager.instance.PlaySFX(12);
                    jumpRemember = 0;
                    anim.Rebind();
                }

                //DashRoll
                if (dashRollCoolTime <= 0f)
                {
                    if (Input.GetKeyDown(KeyCode.C) && isGrounded)
                    {
                        anim.Rebind();
                        anim.SetTrigger("DashRoll");
                        dashRollCounter = dashsRollLength;
                        dashRollCoolTime = timeBetweenDashRoll;
                        PlayerInvincible();
                    }
                }
                else
                {
                    dashRollCoolTime -= Time.deltaTime;
                }
            }
            else
            {
                dashRollCounter -= Time.deltaTime;
                theRB.velocity = new Vector2(dashRollForce * direction, 0f);
                if(!dashRollSound)
                {
                    DustWalk();
                    AudioManager.instance.PlaySFX(10);
                    dashRollSound = true;
                }

                // 대쉬 중간에 점프 버튼을 누르면 대쉬를 중지하고 점프
                if (Input.GetKeyDown(KeyCode.X))
                {
                    dashRollCounter = 0f;
                    theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                    DustJump();
                    AudioManager.instance.StopSFX(10);
                    AudioManager.instance.PlaySFX(12);
                    anim.Rebind();
                }
            }
        }
        else
        {
            pushedBackCounter -= Time.deltaTime;
            //theRB.velocity = new Vector2(-1f * direction * pushedBackForce, 0f);
            theRB.velocity = new Vector2(0f, 0f);
        }

        //if(Input.GetAxisRaw("Horizontal") != 0 && dustCounter < 0f)
        //{
        //    if(isGrounded)
        //    {
        //        DustWalk();
        //        dustCounter = timeBetweenDust;
        //    }
        //}
        //else
        //{
        //    dustCounter -= Time.deltaTime;
        //}

        SetAnimationState();
        //LightFollowing();
    }

    void SetAnimationState()
    {
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("IsGrounded", true);

        }
        else if (Input.GetAxisRaw("Horizontal") != 0 && Mathf.Abs(theRB.velocity.y) <= .1f)
        {
            anim.SetBool("isWalking", true);
            anim.SetBool("IsGrounded", true);
        }

        if (Mathf.Abs(theRB.velocity.y) < 0.1f && isGrounded)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
            anim.SetBool("IsGrounded", true);
        }
        else if (theRB.velocity.y > 0.1f)
        {
            anim.SetBool("isJumping", true);
            anim.SetBool("isFalling", false);
            anim.SetBool("IsGrounded", false);
        }
        else if (theRB.velocity.y < 0)

        {
            anim.SetBool("isFalling", true);
            anim.SetBool("isJumping", false);
            anim.SetBool("IsGrounded", false);
        }
    }
    public void PushedBackTrigger(float force)
    {
        pushedBackForce = force;
        pushedBackCounter = pushedBackTime;
    }

    void PlayerInvincible()
    {
        gameObject.layer = 16;  //PlayerInvincible
        //theBox.enabled = false;
        //theCircle.enabled = false;
    }

    void PlayerPlayer()
    {
        gameObject.layer = 8;  //Player
        //theBox.enabled = true;
        //theCircle.enabled = true;
    }

    public void Parried()
    {
        anim.SetBool("Parried", true);
        PushedBackTrigger(parriedBackForce);
    }

    void LightFollowing() // 플레이어에 parent시키면 방향을 바꿀 때 같이 뒤집어져서 캐릭터 뒷면으로 가버림. 로테이션은 그대로 가고 위치만 추적. 
    {
        
        _candle.transform.position = new Vector3(candlePoint.position.x, candlePoint.position.y, _candle.transform.position.z);
    }

    void DustJump()
    {
        Instantiate(dustJump, dustPoint.position, dustPoint.rotation);
    }

    public void DustLand()
    {
        Instantiate(dustLand, dustPoint.position, dustPoint.rotation);
        
    }

    public void DustWalk()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, LayerMask.GetMask("Ground"));
        Instantiate(dustWalk, hitInfo.point, dustPoint.rotation);
    }

    // Player_Land 애니메이션에서 animation event로 호출
    public void PlaySfxLand()
    {
        AudioManager.instance.PlaySFX(11);
    }
}
