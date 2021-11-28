using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageMelee : MonoBehaviour
{
    public GameObject enemyShooterDieEffect, goulDieEffect, goulHead;
    public GameObject goulDieAnim;
    public Transform dieEffectPoint, goulDieEffectPoint, goulDieEffectpointOffset, goulHeadPoint;
    public float headUpForce;
    private bool isDead; // �� Instantiate�� �� �� �����Ǵ��� �𸣰ڴ�. �ϴ� isDead �÷��׷� ������.

    public void EnemyDie()
    {
        if(!isDead)
        {
            Instantiate(enemyShooterDieEffect, dieEffectPoint.position, dieEffectPoint.rotation);
            Instantiate(goulDieEffect, goulDieEffectPoint.position, goulDieEffectPoint.rotation);
            Instantiate(goulDieEffect, goulDieEffectpointOffset.position, goulDieEffectpointOffset.rotation);
            Instantiate(goulDieAnim, transform.position, transform.rotation);
            Instantiate(goulHead, goulHeadPoint.position, goulHeadPoint.rotation);
            isDead = true;
            CameraShake.instance.CamShakeA();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // �÷��̾��� ���ݿ� ���� ���ó���� EnemyMelee���� ���� ó��
    {
        //�ݻ�� ź���� ������ ���ó��
        if (collision.CompareTag("ProjectileEnemyHit"))
        {
            AudioManager.instance.PlaySFX(2);
            EnemyDie();
        }

        //dead zone �� ���� ���
        if (collision.CompareTag("DeadZone"))
        {
            AudioManager.instance.PlaySFX(2);
            Destroy(gameObject);
        }
    }
}
