using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zzzSuperJump : MonoBehaviour
{
    public float jumpForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerController.instance.theRB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
