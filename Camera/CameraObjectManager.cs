using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObjectManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Respawn"))
        {
            for(int i = 0; i < collision.transform.childCount; i++)
            {
                collision.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Respawn"))
        {
            for(int i = 0; i < collision.transform.childCount; i++)
            {
                collision.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
