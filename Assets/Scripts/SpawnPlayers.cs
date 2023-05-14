using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using Photon.Chat;
using TMPro;
using Photon.Pun.Demo.PunBasics;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject PlayerManager;
    public GameObject PlayerPrefab;

    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;

    public void Start()
    {
        Vector2 randomPosition = new Vector3(Random.Range(minX, maxX), 1.15f, Random.Range(minZ, maxZ));
        PhotonNetwork.Instantiate(PlayerPrefab.name, randomPosition, Quaternion.identity);
    }
}
