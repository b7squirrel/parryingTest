using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public static CheckPoint instance;

    public Animator anim;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            CheckPointController.instance.DeactivateCheckPoint();
            anim.SetBool("On", true);
            CheckPointController.instance.SetSpawnPoint(transform.position);
        }
    }

    public void ResetCheckPoint()
    {
        anim.SetBool("On", false);
    }    
}
