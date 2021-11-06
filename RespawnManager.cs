using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;

    public float respawnTime;

    public GameObject playerDieEffect;

    public Transform respawnPoint;

    public bool isPlayerDead; // 적이 패링할 때 플레이어가 죽어 있으면 플레이어에게 parried를 시키는 등의 행동을 하지 않도록

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
        
        // 임시작업. 첵포인트 설정을 하면 수정해야 함. 
        PlayerController.instance.transform.position = respawnPoint.position;
        
        
        //패링하다가 죽으면 뒤로 밀려나면서 리스폰 되는 현상을 막아줌
        PlayerController.instance.pushedBackCounter = 0f;
        PlayerParrying.instance.parryingPoint.gameObject.SetActive(false);
        PlayerAttack.instance.attackPoint.gameObject.SetActive(false);
        isPlayerDead = false;
    }
}
