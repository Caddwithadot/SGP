using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class NewRoomManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField playerDisplayName;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom("a");
    }

    public void JoinRoom()
    {
        string nickname = playerDisplayName.text;

        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.Name == "a")
        {
            foreach (KeyValuePair<int, Player> playerEntry in PhotonNetwork.CurrentRoom.Players)
            {
                Debug.Log(playerEntry.Value.NickName);

                if (playerEntry.Value.NickName == nickname)
                {
                    Debug.Log("Nickname is already in use.");
                    return;
                }
            }
        }

        PhotonNetwork.NickName = nickname;
        //PhotonNetwork.JoinRoom("a");
    }

    public void CheckPlayerList()
    {
        if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.Name == "a")
        {
            // Get the list of players in the target room
            Player[] players = PhotonNetwork.PlayerList;

            Debug.Log("Players in room 'a':");

            // Iterate through the player list and log each player's nickname
            foreach (Player player in players)
            {
                Debug.Log(player.NickName);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}