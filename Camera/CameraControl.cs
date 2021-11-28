using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static CameraControl instance;
    public Transform target;
    public Vector3 offset;
    private Vector3 targetPosition;

    public float minHeight, maxHeight, minWidth, maxWidth;
    private float clampedY;

    public bool stopFollow;


    private void Awake()
    {
        instance = this;
        minWidth = -22f;
        maxWidth = 138f;
        transform.position = new Vector3(Mathf.Clamp(target.position.x, minWidth, maxWidth), Mathf.Clamp(target.position.y, minHeight, maxHeight), transform.position.z);
        transform.position += offset;
    }

    private void LateUpdate()
    {
        targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        targetPosition += offset;

        if(!stopFollow)
        {
            if(Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(Mathf.Clamp(targetPosition.x, minWidth, maxWidth), Mathf.Clamp(targetPosition.y, minHeight, maxHeight), transform.position.z);
                
            }
            
        }
        else
        {
            Debug.Log("STOP FOLLOWING");
        }
    }
}
