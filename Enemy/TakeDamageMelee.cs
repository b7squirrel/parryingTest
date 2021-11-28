using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageMelee : MonoBehaviour
{
    public GameObject enemyShooterDieEffect, goulDieEffect, goulHead;
    public GameObject goulDieAnim;
    public Transform dieEffectPoint, goulDieEffectPoint, goulDieEffectpointOffset, goulHeadPoint;
    public float headUpForce;
    private bool isDead; // 왜 Instantiate이 두 번 생성되는지 모르겠다. 일단 isDead 플래그로 제어함.

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

    private void OnTriggerEnter2D(Collider2D collision) // 플레이어의 공격에 의한 사망처리는 EnemyMelee에서 직접 처리
    {
        //반사된 탄알이 닿으면 사망처리
        if (collision.CompareTag("ProjectileEnemyHit"))
        {
            AudioManager.instance.PlaySFX(2);
            EnemyDie();
        }

        //dead zone 에 들어가면 사망
        if (collision.CompareTag("DeadZone"))
        {
            AudioManager.instance.PlaySFX(2);
            Destroy(gameObject);
        }
    }
}
