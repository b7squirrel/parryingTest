using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("FrameRate")]
    private int target;

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        //Quit
        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
    }

    // 프레임레이트
    private void FixedUpdate()
    {
        if (target != Application.targetFrameRate)
        {
            Application.targetFrameRate = target;
        }
    }
    public float Choose (float[] probs)
    {
        float total = 0f;

        foreach(float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for(int i = 0; i < probs.Length; i++)
        {
            if(randomPoint < probs[i])
            {
                return i;
            }else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }
}
