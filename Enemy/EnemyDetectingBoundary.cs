using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectingBoundary : MonoBehaviour
{
    public bool isDetecting;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isDetecting = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isDetecting = false;
        }
    }
}
