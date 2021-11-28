using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTemp : MonoBehaviour
{
    public Transform target;
    //public Transform middleBackground, farBackground;
    // private float lastXpos;
    private Vector2 lastPos;
    public float maxHeight, minHeight;

    public float offsetX;

    // Start is called before the first frame update
    void Start()
    {
        // lastXpos = transform.position.x;
        lastPos = new Vector2(transform.position.x + offsetX, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        float clampedY = Mathf.Clamp(transform.position.y, minHeight, maxHeight);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z); 

        transform.position = new Vector3(target.position.x + offsetX, transform.position.y, transform.position.z);

        // float amountMove = transform.position.x - lastXpos;
        Vector2 amountMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);
        //middleBackground.position += new Vector3(amountMove.x, amountMove.y, 0f) * .5f;
        //farBackground.position += new Vector3(amountMove.x, amountMove.y, 0f);

        lastPos = new Vector2(transform.position.x + offsetX, transform.position.y);
    }
}
