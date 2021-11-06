using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private BoxCollider2D camBox;
    private Camera cam;
    private float sizeX, sizeY, ratio;

    private void Start()
    {
        camBox = GetComponent<BoxCollider2D>();
        cam = GetComponent<Camera>();

        sizeY = cam.orthographicSize * 2;
        ratio = (float)Screen.width / (float)Screen.height;
        sizeX = sizeY * ratio;

        camBox.size = new Vector2(sizeX, sizeY);
    }
}
