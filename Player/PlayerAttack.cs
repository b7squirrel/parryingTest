using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack instance;

    public GameObject attackPoint;
    public Animator anim;
    public float attackJumpForce;

    [Header("Swing Effect")]
    public GameObject swingEffect;
    public Transform swingEffectPoint;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("Player_CAttack"))
        {
            attackPoint.gameObject.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Z) && PlayerController.instance.pushedBackCounter <= 0f)
            {
                Attack();
            }

            if(Input.GetKeyDown(KeyCode.Z) && PlayerParrying.instance.counterAtkPossible)
            {
                CounterAttack();
            }
        }
    }
    void Attack()
    {
        attackPoint.gameObject.SetActive(true);
        attackPoint.gameObject.tag = "PlayerAttackBox"; // ��־��� �±�
        Debug.Log(attackPoint.gameObject.tag);

        anim.SetTrigger("Attack");
        var clone = Instantiate(swingEffect, swingEffectPoint.position, swingEffectPoint.rotation);
        clone.transform.parent = PlayerController.instance.transform;

        if (PlayerController.instance.isGrounded)
        {
            PlayerController.instance.theRB.AddForce(Vector2.up * attackJumpForce, ForceMode2D.Impulse);
        }
        
        AudioManager.instance.PlaySFX(7);
        PlayerParrying.instance.counterAtkPossible = false;  // ī���;��� ���� ���θ� false�� �ʱ�ȭ
    }

    void CounterAttack()
    {
        attackPoint.gameObject.SetActive(true);
        attackPoint.gameObject.tag = "PlayerCAttackBox";  // ī���� ���� �±�
        Debug.Log(attackPoint.gameObject.tag);

        anim.SetTrigger("CAttack");
        //var clone = Instantiate(swingEffect, swingEffectPoint.position, swingEffectPoint.rotation);
        //clone.transform.parent = PlayerController.instance.transform;


        if (PlayerController.instance.isGrounded)
        {
            PlayerController.instance.theRB.AddForce(Vector2.up * attackJumpForce, ForceMode2D.Impulse);
        }

        AudioManager.instance.PlaySFX(7);
        PlayerParrying.instance.counterAtkPossible = false;  // ī���;��� ���� ���θ� false�� �ʱ�ȭ
    }
}
