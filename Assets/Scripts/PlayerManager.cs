using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string kickedPlayerNickname;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            int playerID = GameObject.Find(kickedPlayerNickname).GetComponent<PhotonView>().ViewID;

            GetComponent<PhotonView>().RPC("KickAndLoadScene", RpcTarget.All, new object[] {playerID});
        }
    }

    [PunRPC]
    private void KickAndLoadScene(int player)
    {
        Debug.Log(player);

        // Make sure we're not kicking ourselves
        if (PhotonNetwork.LocalPlayer.NickName == kickedPlayerNickname)
        {
            Debug.Log("You cannot kick yourself!");
        }
        else
        {
            // Kick the player from the room
            Debug.Log($"Player {kickedPlayerNickname} has been kicked from the room!");

            // Load the kicked scene for this player only
            SceneManager.LoadScene("Kicked");
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CustomEvent.Trigger(this.gameObject, "PlayerLeft", otherPlayer.NickName);
    }
}