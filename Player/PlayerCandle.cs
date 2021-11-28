using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class PlayerCandle : MonoBehaviour
{
    private float direction;
    
    void Update()
    {
        // ������ �� �к��� ����
        direction = Input.GetAxisRaw("Horizontal");
        if(direction == 0)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if(direction == 1)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 30f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, -30f);
        }
    }
}
