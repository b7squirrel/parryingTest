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
                // �÷��̾��� ����� �и��ڽ��� ��ź�� ��ġ����. ����� �ڿ��� ��ź�� �´� ��츦 ��Ÿ����. 
                float checkDirections = (transform.position.x - collision.transform.position.x) * PlayerController.instance.direction;

                //�и��� ���°� �ƴ� ��� ��, �� ��ź ��ο��� �ǰ� ó��
                if (!PlayerParrying.instance.isParrying)
                {
                    RespawnManager.instance.RespawnPlayer();
                }
                else if (PlayerParrying.instance.isParrying) // �и��� �������� ��� �ڿ��� ���� ��ź�� ��츸 �ǰ����� ó��
                {
                    if (checkDirections > 0f)
                    {
                        RespawnManager.instance.RespawnPlayer();
                    }
                }
            }
        }else if(collision.CompareTag("DeadZone")) // invincible ���¶� �������� ����
        {
            RespawnManager.instance.RespawnPlayer();
        }
    }
}
