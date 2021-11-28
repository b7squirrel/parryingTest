using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zzzHeadUp : MonoBehaviour
{
    public float upForce;
    public Rigidbody2D theRB;
    public GameObject deadEffect;

    private float hitCounter;

    private void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        theRB.AddForce(Vector2.up * upForce, ForceMode2D.Impulse);
        theRB.AddTorque(-2, ForceMode2D.Impulse);
        hitCounter = .3f; // �������� ���� �����ִ� �÷��̾��� ����collider�� �ε��� �ı��Ǵ� ���� ����
    }

    private void Update()
    {
        hitCounter -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(hitCounter < 0f)
        {
            if (collision.CompareTag("ProjectileEnemyHit") || 
                collision.CompareTag("PlayerAttackBox") || 
                collision.CompareTag("ProjectileEnemy") ||
                collision.CompareTag("EnemyAttackBox"))
            {
                Instantiate(deadEffect, transform.position, transform.rotation);
                AudioManager.instance.PlaySFX(3);
                AudioManager.instance.PlaySFX(16);
                Destroy(gameObject);
            }
        }
    }
}
