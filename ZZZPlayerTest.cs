using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZZZPlayerTest : MonoBehaviour
{
    private float moveSpeed = 8f;
    private Rigidbody2D theRB;
    public GameObject sparkEffect;

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal"), theRB.velocity.y);
    }

    
}
