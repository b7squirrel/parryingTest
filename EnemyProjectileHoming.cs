using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileHoming : MonoBehaviour
{
    public float moveSpeed;
    //public float parryiedSpeed;

    private Rigidbody2D theRB;
    private Vector2 direction;

    //ź���� �÷��̾� ���ʿ� �ִ� ���¿��� �и��Ǿ��� ���, ���� ���� ��� ������
    private float fixingDirection;

    [HideInInspector]
    public Vector2 initialPoint;
    
    public float homingTime;
    private Vector2 initialVelocityOfParrying;
    private Transform contactPoint;

    //public GameObject parryingSpark;

    public GameObject projectileDieEffect;

    private bool isParryied;

    public GameObject smokeOfProjectile;
    public GameObject smokeOfProjectileB;



    //�÷��̾ aiming �ϱ� ���� ����
    private Transform player;
    private Vector2 moveDirection;


    public float pushingBackForce;
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        theRB.gravityScale = 1f;

        //initial Point �ʱ�ȭ, ������ ���� �ƴ϶� �������� ������ ���� ���ư����� y������ 0.5 ��ŭ �����־���. 
        initialPoint = new Vector2(transform.position.x, transform.position.y -1f);

        isParryied = false;
        direction = transform.right * -1f;
        this.gameObject.layer = 12;
        this.gameObject.tag = "ProjectileEnemy";

        moveDirection = (PlayerController.instance.transform.position - transform.position).normalized * moveSpeed;
    }
    void Update()
    {
        // Movement
        if (!isParryied)
        {
            //theRB.velocity = direction * moveSpeed;
            theRB.velocity = new Vector2(moveDirection.x, moveDirection.y);

            Instantiate(smokeOfProjectile, transform.position, Quaternion.identity);
        }
        else if (isParryied)
        {
            Instantiate(smokeOfProjectile, transform.position, Quaternion.identity);
            Instantiate(smokeOfProjectileB, transform.position, Quaternion.identity);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("ParryingBox"))
            {
                isParryied = true;
                theRB.gravityScale = 1f;
                this.gameObject.layer = 13;
                this.gameObject.tag = "ProjectileEnemyHit";

                // �÷��̾� �ڷ� �б�
                PlayerController.instance.PushedBackTrigger(pushingBackForce);
                CameraShake.instance.CamShakeA();

                ///����ũ ����Ʈ
                //Instantiate(parryingSpark, transform.position, transform.rotation);

                // �и��� �ʱ� ��ġ ����
                contactPoint = transform;

                // �ʱ� �ӵ� ����
                initialVelocityOfParrying = CalculateVelecity(initialPoint, contactPoint.position, homingTime);

                // �÷��̾ ������ ź���� Parrying ���� ��� źȯ�� ���� �������� Parrying ��Ű��
                if (Vector2.Distance(initialPoint, transform.position) > Vector2.Distance(initialPoint, PlayerController.instance.transform.position))
                {
                    fixingDirection = -1f;
                }
                else
                {
                    fixingDirection = 1f;
                }

                //theRB.AddForce(initialVelocityOfParrying * fixingDirection, ForceMode2D.Impulse);
                theRB.velocity = initialVelocityOfParrying * fixingDirection;
            }

            // Player, Enemy �� Ground�� �浹�ϸ� �ı�
            if (collision.CompareTag("Player") || collision.CompareTag("Enemy") || collision.CompareTag("Ground"))
            {
                DestroyProjectile();
            }
        }
    }

    void DestroyProjectile()
    {
        Instantiate(projectileDieEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    Vector2 CalculateVelecity(Vector2 _target, Vector2 _origin, float time)
    {
        Vector2 distance = _target - _origin;

        float Vx = distance.x / time;
        float Vy = distance.y / time + 0.5f * Mathf.Abs(Physics2D.gravity.y) * time;

        Vector2 result;
        result.x = Vx;
        result.y = Vy;

        return result;
    }
}
