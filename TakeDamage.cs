using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public GameObject enemyShooterDieEffect;
    public Transform dieEffectPoint;
    public void EnemyDie()
    {
        Instantiate(enemyShooterDieEffect, dieEffectPoint.position, dieEffectPoint.rotation);
        
        CameraShake.instance.CamShakeA();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어의 attack box가 닿거나 반사된 탄알이 닿으면 사망처리
        if (collision.CompareTag("PlayerAttackBox") || collision.CompareTag("ProjectileEnemyHit"))
        {
            AudioManager.instance.StopSFX(15);
            AudioManager.instance.PlaySFX(3);
            EnemyDie();
        }

        //dead zone 에 들어가면 사망
        if(collision.CompareTag("DeadZone"))
        {
            AudioManager.instance.PlaySFX(3);
            EnemyDie();
        }

    }
}
