using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_Text _roomName;
    private TMP_Text RoomName
    {
        get { return _roomName; }
    }

    public TMP_InputField nameInput;

    public void OnClick_CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 4 };

        if (PhotonNetwork.CreateRoom(RoomName.text, roomOptions, TypedLobby.Default))
        {
            print("create room successfully sent.");
            PhotonNetwork.NickName = nameInput.text;
        }
        else
        {
            print("create room failed to send");
        }
    }


    public override void OnCreatedRoom()
    {
        print("Room created successfully");
    }
}
