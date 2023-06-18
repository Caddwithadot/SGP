using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyNetwork : MonoBehaviourPunCallbacks
{
    public TMP_InputField nameInput;

    private void Start()
    {
        print("Connecting to server..");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("Connected to master.");

        //If comment on PlayerNetwork.cs and set this to TRUE
        PhotonNetwork.AutomaticallySyncScene = false;

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("Joined lobby.");

        if (!PhotonNetwork.InRoom)
        {
            MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();
        }
    }
}
