using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemymeleeHitBox : MonoBehaviour
{
    public bool parried;
    public float pushingBackForce;

    //�и��� �Ǿ��ٸ� State Machine���� ���θ� ������.(parried ���� ���ؼ�)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("ParryingBox"))
        {
            PlayerController.instance.PushedBackTrigger(pushingBackForce);
            parried = true;
        }
    }
}
