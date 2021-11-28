using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemymeleeHitBox : MonoBehaviour
{
    public bool parried;
    public float pushingBackForce;

    //패리가 되었다면 State Machine에서 여부를 가져감.(parried 값을 통해서)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("ParryingBox"))
        {
            PlayerController.instance.PushedBackTrigger(pushingBackForce);
            parried = true;
        }
    }
}
