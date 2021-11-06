using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeWarp : MonoBehaviour
{
    public static TimeWarp instance;

    [Header("TimeStop")]
    public float stopTime;
    private bool isStopping;
    public float slowTime;
    public float delayTime;

    

    private void Awake()
    {
        instance = this;
    }

    public void TimeStop()
    {
        if (!isStopping)
        {
            isStopping = true;
            StartCoroutine(Delay());
            Time.timeScale = 0;

            StartCoroutine("Stop");
            StartCoroutine("CamShake");
        }
    }

    IEnumerator Stop()
    {
        yield return new WaitForSecondsRealtime(stopTime);
        Time.timeScale = 0.02f;

        yield return new WaitForSecondsRealtime(slowTime);

        Time.timeScale = 1;
        isStopping = false;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(delayTime);
    }    

    
}
