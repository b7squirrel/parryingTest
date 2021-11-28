using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer_temp : MonoBehaviour
{
    public float bounceForce;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerController.instance.theRB.velocity = new Vector2(PlayerController.instance.theRB.velocity.x, bounceForce);
        }
    }
}
