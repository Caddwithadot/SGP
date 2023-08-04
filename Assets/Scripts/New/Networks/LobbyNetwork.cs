using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyNetwork : MonoBehaviourPunCallbacks
{
    public bool amRefreshing = false;
    public GameObject refreshScreen;

    private void Start()
    {
        print("Connecting to server..");
        PhotonNetwork.ConnectUsingSettings();

        refreshScreen.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        print("Connected to master.");

        PhotonNetwork.AutomaticallySyncScene = false;

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("Joined lobby.");
        refreshScreen.SetActive(false);

        if (!PhotonNetwork.InRoom)
        {
            MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();
        }

        if (amRefreshing)
        {
            OnClickRefresh();
        }
    }

    public void OnClickRefresh()
    {
        amRefreshing = false;
        refreshScreen.SetActive(true);

        PhotonNetwork.Disconnect();
        Debug.Log("Disconnected");

        Debug.Log("Reconnecting...");
        PhotonNetwork.ConnectUsingSettings();
    }
}
