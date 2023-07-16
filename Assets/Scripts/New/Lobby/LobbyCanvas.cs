using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyCanvas : MonoBehaviour
{
    public TMP_InputField nameInput;

    public void OnClickJoinRoom(string roomName)
    {
        if (PhotonNetwork.JoinRoom(roomName))
        {
            PhotonNetwork.NickName = nameInput.text;
        }
        else
        {
            print("Join room failed.");
        }
    }
}
