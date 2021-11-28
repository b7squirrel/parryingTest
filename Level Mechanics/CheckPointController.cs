using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    public static CheckPointController instance;
    private CheckPoint[] checkPoints;

    public Vector3 spawnPoint;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        checkPoints = FindObjectsOfType<CheckPoint>();
        spawnPoint = PlayerController.instance.transform.position;
    }

    public void DeactivateCheckPoint()
    {
        for(int i = 0; i < checkPoints.Length; i++)
        {
            checkPoints[i].ResetCheckPoint();
        }
    }
    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }
}
