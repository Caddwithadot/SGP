using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using Photon.Chat;
using TMPro;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject PlayerManager;
    public GameObject PlayerPrefab;

    [Header("Spawning Player Pos.")]
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;

    [Header ("For Kicking Players.")]
    public string targetPlayerNickname;
    public string targetPhotonId;
    Nametag nametag;

    public void Start()
    {
        Vector2 randomPosition = new Vector3(Random.Range(minX, maxX), 1.15f, Random.Range(minZ, maxZ));
        GameObject obj = PhotonNetwork.Instantiate(PlayerPrefab.name, randomPosition, Quaternion.identity);
        nametag = obj.GetComponentInChildren<Nametag>();

        Debug.Log(nametag.name);
    }

    private string GetPlayerIdByNickname(string nickname)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.NickName == nickname)
            {
                return player.UserId;
            }
        }

        Debug.LogError($"Player with nickname '{nickname}' not found!");
        return null;
    }

    private void Update()
    {
        // Check if the sendPlayerIdKey is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            string playerId = GetPlayerIdByNickname(targetPlayerNickname);

            nametag.GetKicked(playerId);
        }
    }
}
