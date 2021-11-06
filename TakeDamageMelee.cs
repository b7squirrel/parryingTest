using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageMelee : MonoBehaviour
{
    public GameObject enemyShooterDieEffect;
    public Transform dieEffectPoint;

    public void EnemyDie()
    {
        Instantiate(enemyShooterDieEffect, dieEffectPoint.position, dieEffectPoint.rotation);

        CameraShake.instance.CamShakeA();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) // �÷��̾��� ���ݿ� ���� ���ó���� EnemyMelee���� ���� ó��
    {
        //�ݻ�� ź���� ������ ���ó��
        if (collision.CompareTag("ProjectileEnemyHit"))
        {
            AudioManager.instance.PlaySFX(3);
            EnemyDie();
        }

        //dead zone �� ���� ���
        if (collision.CompareTag("DeadZone"))
        {
            AudioManager.instance.PlaySFX(3);
            Destroy(gameObject);
        }
    }
}
