using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryVertical : MonoBehaviour
{
    public float minHeight, maxHeight, minWidth, maxWidth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            CameraControl.instance.minHeight = minHeight;
            CameraControl.instance.maxHeight = maxHeight;
            CameraControl.instance.minWidth = minWidth;
            CameraControl.instance.maxWidth = maxWidth;
        }
    }
}
