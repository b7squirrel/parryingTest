using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : MonoBehaviour
{
    public GameObject bossAreaTrigger;

    void Start()
    {
        bossAreaTrigger.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            bossAreaTrigger.gameObject.SetActive(true);
        }
    }
}
