using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float moveSpeed;
    public float parryiedSpeed;

    private Rigidbody2D theRB;
    private Vector2 direction;

    public GameObject parryingEffect;

    public GameObject projectileDieEffect;

    private bool isParryied;

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        theRB.gravityScale = .3f;

        isParryied = false;
        direction = transform.right * -1f;
        transform.forward = direction.normalized;
        this.gameObject.layer = 12;
    }

    void Update()
    {
        if(isParryied)
        {
            transform.position += transform.forward * parryiedSpeed * Time.deltaTime;
        }
        else if(!isParryied)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision != null)
        {
            if (collision.transform.CompareTag("ParryingBox"))
            {
                isParryied = true;
                theRB.gravityScale = 1.5f;
                this.gameObject.layer = 13;
                CameraShake.instance.CamShakeA();

                direction = Vector2.Reflect(direction, collision.GetContact(0).normal);
                transform.forward = direction.normalized;
            }

            if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("Ground"))
            {
                DestroyProjectile();
            }
        }
    }

    void DestroyProjectile()
    {
        Instantiate(projectileDieEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
