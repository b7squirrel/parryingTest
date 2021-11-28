using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryFarLeft : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("IN");
            CameraControl.instance.stopFollow = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("OUT");
            CameraControl.instance.stopFollow = false;
        }
    }
}
