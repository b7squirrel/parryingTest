using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public bool isInvincible;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ProjectileEnemy") || collision.CompareTag("EnemyAttackBox"))
        {
            if(!isInvincible)
            {
                // 플레이어의 방향과 패링박스와 총탄의 위치관계. 양수는 뒤에서 총탄을 맞는 경우를 나타낸다. 
                float checkDirections = (transform.position.x - collision.transform.position.x) * PlayerController.instance.direction;

                //패링인 상태가 아닌 경우 앞, 뒤 총탄 모두에게 피격 처리
                if (!PlayerParrying.instance.isParrying)
                {
                    RespawnManager.instance.RespawnPlayer();
                }
                else if (PlayerParrying.instance.isParrying) // 패링을 성공했을 경우 뒤에서 맞은 총탄의 경우만 피격으로 처리
                {
                    if (checkDirections > 0f)
                    {
                        RespawnManager.instance.RespawnPlayer();
                    }
                }
            }
        }else if(collision.CompareTag("DeadZone")) // invincible 상태라도 떨어지면 죽음
        {
            RespawnManager.instance.RespawnPlayer();
        }
    }
}
