using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [HideInInspector]
    public float direction;

    public float moveSpeed;

    private float xDirection;
    public Rigidbody2D theRB;
    public SpriteRenderer theSR;

    private bool isGrounded;
    public LayerMask whatIsGround;
    public Transform groundCheck;

    public float jumpForce;
    public float jumpRememberTime;
    private float jumpRemember;
    
    public float pushedBackTime;
    public float pushedBackCounter;
    private float pushedBackForce;

    //dashRoll variables
    public float dashRollCounter;
    public float dashsRollLength;
    public float dashRollForce;
    public float timeBetweenDashRoll;
    private float dashRollCoolTime;

    //parried variables
    public float parriedBackForce;

    private BoxCollider2D theBox;
    private CircleCollider2D theCircle;

    public Animator anim;

    //[Header("Camera")]
    //public Transform cameraTarget;
    //public float aheadAmount, aheadSpeed;

    [Header("light")]
    public GameObject candle;
    public Transform candlePoint;
    private GameObject _candle;

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
        if (pushedBackCounter <= 0f)
        {
            anim.SetBool("Parried", false);

            if (dashRollCounter <= 0f)
            {
                //  colliders �ʱ�ȭ
                PlayerPlayer();

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

                // Grounded
                isGrounded = Physics2D.OverlapCircle(groundCheck.position, .05f, whatIsGround);

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
                    theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
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

                // Camera Control
                //if (Input.GetAxisRaw("Horizontal") != 0)
                //{
                //    cameraTarget.localPosition = 
                //        new Vector3(aheadAmount * Input.GetAxisRaw("Horizontal"), cameraTarget.localPosition.y, cameraTarget.localPosition.z);
                //}
            }
            else
            {
                dashRollCounter -= Time.deltaTime;
                theRB.velocity = new Vector2(dashRollForce * direction, 0f);

                // �뽬 �߰��� ���� ��ư�� ������ �뽬�� �����ϰ� ����
                if (Input.GetKeyDown(KeyCode.X))
                {
                    dashRollCounter = 0f;
                    theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                    anim.Rebind();
                }
            }
        }
        else
        {
            pushedBackCounter -= Time.deltaTime;
            theRB.velocity = new Vector2(-1f * direction * pushedBackForce, 0f);
        }
        
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

    void LightFollowing() // �÷��̾ parent��Ű�� ������ �ٲ� �� ���� ���������� ĳ���� �޸����� ������. �����̼��� �״�� ���� ��ġ�� ����. 
    {
        
        _candle.transform.position = new Vector3(candlePoint.position.x, candlePoint.position.y, _candle.transform.position.z);
    }
}
