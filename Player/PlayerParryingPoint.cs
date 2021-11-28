using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParryingPoint : MonoBehaviour
{
    public GameObject objectToFollow;

    public GameObject parryingEffect;
    public GameObject parryingSpark;
    public Transform parryingEffectPoint;

    public Transform parryingSparkPosition;
    void Update()
    {
        // 비활성화 되었다가 다시 활성화 될 때 따라오지 못하는 경우를 방지
        transform.position = objectToFollow.transform.position;
    }

    // 적의 attack box가 닿으면 이펙트 발생
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("EnemyAttackBox")|| collision.CompareTag("ProjectileEnemyHit"))
        {
            var clone = Instantiate(parryingEffect, parryingEffectPoint.position, parryingEffectPoint.rotation);
            Instantiate(parryingSpark, parryingSparkPosition.position, parryingSparkPosition.rotation);
            AudioManager.instance.PlaySFX(4);
            CameraShake.instance.CamShakeA();
        }
    }
}
