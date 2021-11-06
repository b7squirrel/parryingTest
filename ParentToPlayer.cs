using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentToPlayer : MonoBehaviour
{
    void Start()
    {
        transform.parent = PlayerController.instance.transform;
    }
}
