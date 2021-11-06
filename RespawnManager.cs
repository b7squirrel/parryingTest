using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;

    public float respawnTime;

    public GameObject playerDieEffect;

    public Transform respawnPoint;

    public bool isPlayerDead; // ���� �и��� �� �÷��̾ �׾� ������ �÷��̾�� parried�� ��Ű�� ���� �ൿ�� ���� �ʵ���

    private void Awake()
    {
        instance = this;
    }

    public void RespawnPlayer()
    {
        CameraShake.instance.CamShakeA();

        StartCoroutine(RespawnPlayerCo());
    }

    public IEnumerator RespawnPlayerCo()
    {
        isPlayerDead = true;
        AudioManager.instance.PlaySFX(3);
        Instantiate(playerDieEffect, PlayerController.instance.transform.position, Quaternion.identity);
        
        CameraShake.instance.CamShakeA();

        PlayerController.instance.anim.Rebind();
        PlayerController.instance.gameObject.SetActive(false);
        yield return new WaitForSeconds(respawnTime);

        PlayerController.instance.gameObject.SetActive(true);
        
        // �ӽ��۾�. ý����Ʈ ������ �ϸ� �����ؾ� ��. 
        PlayerController.instance.transform.position = respawnPoint.position;
        
        
        //�и��ϴٰ� ������ �ڷ� �з����鼭 ������ �Ǵ� ������ ������
        PlayerController.instance.pushedBackCounter = 0f;
        PlayerParrying.instance.parryingPoint.gameObject.SetActive(false);
        PlayerAttack.instance.attackPoint.gameObject.SetActive(false);
        isPlayerDead = false;
    }
}
