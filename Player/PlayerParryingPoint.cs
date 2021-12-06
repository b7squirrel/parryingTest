using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParryingPoint : MonoBehaviour
{
    public static PlayerParryingPoint instance;

    public GameObject objectToFollow;

    public GameObject parryingEffect;
    public GameObject parryingSpark;
    public Transform parryingEffectPoint;

    public Transform parryingSparkPosition;

    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        // ��Ȱ��ȭ �Ǿ��ٰ� �ٽ� Ȱ��ȭ �� �� ������� ���ϴ� ��츦 ����
        transform.position = objectToFollow.transform.position;
    }

    // ���� attack box�� ������ ����Ʈ �߻�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("EnemyAttackBox"))  // ���� ���� ������ �и��ϸ� ī���� ������ �����ϰ� ��(counterAtkPossible)
        {
            var clone = Instantiate(parryingEffect, parryingEffectPoint.position, parryingEffectPoint.rotation);
            Instantiate(parryingSpark, parryingSparkPosition.position, parryingSparkPosition.rotation);
            AudioManager.instance.PlaySFX(4);
            CameraShake.instance.CamShakeA();
            PlayerParrying.instance.counterAtkPossible = true;
        }
        if (collision.CompareTag("ProjectileEnemyHit"))  // ���� ���Ÿ� ������ �и��ϸ� ī���� ������ �ߵ����� ����
        {
            var clone = Instantiate(parryingEffect, parryingEffectPoint.position, parryingEffectPoint.rotation);
            Instantiate(parryingSpark, parryingSparkPosition.position, parryingSparkPosition.rotation);
            AudioManager.instance.PlaySFX(4);
            CameraShake.instance.CamShakeA();
        }
    }
}
