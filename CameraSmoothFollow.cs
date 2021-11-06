using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    public static CameraSmoothFollow instance;

    [Header("Follow")]
    public Vector3 offset;
    public Transform target;
    public float smoothSpeed;
    private Vector3 desiredPosition, smoothedPosition;

    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        desiredPosition = target.position + offset;
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
