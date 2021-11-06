using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileHoming : MonoBehaviour
{
    public float moveSpeed;
    //public float parryiedSpeed;

    private Rigidbody2D theRB;
    private Vector2 direction;

    //탄알이 플레이어 뒤쪽에 있는 상태에서 패링되었을 경우, 가던 길을 계속 가도록
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



    //플레이어를 aiming 하기 위한 변수
    private Transform player;
    private Vector2 moveDirection;


    public float pushingBackForce;
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        theRB.gravityScale = 1f;

        //initial Point 초기화, 지팡이 끝이 아니라 마법사의 몸통을 향해 날아가도록 y값에서 0.5 만큼 더해주었음. 
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

                // 플레이어 뒤로 밀기
                PlayerController.instance.PushedBackTrigger(pushingBackForce);
                CameraShake.instance.CamShakeA();

                ///스파크 이펙트
                //Instantiate(parryingSpark, transform.position, transform.rotation);

                // 패링시 초기 위치 정의
                contactPoint = transform;

                // 초기 속도 정의
                initialVelocityOfParrying = CalculateVelecity(initialPoint, contactPoint.position, homingTime);

                // 플레이어를 지나간 탄알을 Parrying 했을 경우 탄환의 진행 방향으로 Parrying 시키기
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

            // Player, Enemy 나 Ground에 충돌하면 파괴
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
