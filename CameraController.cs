using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerController player;
    public BoxCollider2D boundsBox;

    public float resolutionY;
    private float halfHeight, halfWidth;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();

        //halfHeight = Camera.main.orthographicSize;
        //halfWidth = halfHeight * Camera.main.aspect;

        halfHeight = resolutionY * .5f / 16f; 
        halfWidth = halfHeight * Camera.main.aspect;
    }

    private void Update()
    {
        if(player != null)
        {
            //transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);

            transform.position = new Vector3(
                Mathf.Clamp(player.transform.position.x, boundsBox.bounds.min.x + halfWidth, boundsBox.bounds.max.x - halfWidth),
                Mathf.Clamp(player.transform.position.y, boundsBox.bounds.min.y + halfHeight, boundsBox.bounds.max.y - halfHeight),
                transform.position.z);
        }
    }

}
