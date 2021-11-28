using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack instance;

    public GameObject attackPoint;
    public Animator anim;
    public bool isAttacking;
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
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack"))
        {
            attackPoint.gameObject.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Z) && PlayerController.instance.pushedBackCounter <= 0f)
            {
                anim.SetTrigger("Attack");
                var clone = Instantiate(swingEffect, swingEffectPoint.position, swingEffectPoint.rotation);
                clone.transform.parent = PlayerController.instance.transform;

                isAttacking = true;
                Attack();
            }
        }
    }
    void Attack()
    {
        attackPoint.gameObject.SetActive(true);
        if(PlayerController.instance.isGrounded)
        {
            PlayerController.instance.theRB.AddForce(Vector2.up * attackJumpForce, ForceMode2D.Impulse);
        }
        
        AudioManager.instance.PlaySFX(7);
        //isAttacking = false;
    }
}
