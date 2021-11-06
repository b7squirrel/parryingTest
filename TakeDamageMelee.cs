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

    private void OnTriggerEnter2D(Collider2D collision) // 플레이어의 공격에 의한 사망처리는 EnemyMelee에서 직접 처리
    {
        //반사된 탄알이 닿으면 사망처리
        if (collision.CompareTag("ProjectileEnemyHit"))
        {
            AudioManager.instance.PlaySFX(3);
            EnemyDie();
        }

        //dead zone 에 들어가면 사망
        if (collision.CompareTag("DeadZone"))
        {
            AudioManager.instance.PlaySFX(3);
            Destroy(gameObject);
        }
    }
}
