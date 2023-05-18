using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField playerDisplayName;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom("a");
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("a");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.NickName = playerDisplayName.text;

        PhotonNetwork.LoadLevel("Game");
    }
}
