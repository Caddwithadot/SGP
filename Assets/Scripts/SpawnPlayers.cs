using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject PlayerPrefab;

    [Header("Spawning Player Pos.")]
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;

    private void Start()
    {
        Vector2 randomPosition = new Vector3(UnityEngine.Random.Range(minX, maxX), 1.15f, UnityEngine.Random.Range(minZ, maxZ));
        PhotonNetwork.Instantiate(PlayerPrefab.name, randomPosition, Quaternion.identity);
    }
}
