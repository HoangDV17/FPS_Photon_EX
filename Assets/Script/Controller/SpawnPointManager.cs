using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : SingletonMonoBehavier<SpawnPointManager>
{
    SpawnPoint[] spawnPoints;
    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<SpawnPoint>();
    }

    public Transform GetSpawnpoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
    }
}
