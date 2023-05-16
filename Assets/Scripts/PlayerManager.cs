using Photon.Chat;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
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

            GetComponent<PhotonView>().RPC("Kick", RpcTarget.All, new object[] { playerID });
        }

        if (Variables.Object(this).Get<string>("activateConfirmFor") != null)
        {
            int playerID = Variables.Object(this).Get<int>("currentViewID");

            GetComponent<PhotonView>().RPC("ConfirmPrompt", RpcTarget.All, new object[] { playerID });
        }

        if (Variables.Object(this).Get<string>("activateDenyFor") != null)
        {
            int playerID = Variables.Object(this).Get<int>("currentViewID");

            GetComponent<PhotonView>().RPC("DenyPrompt", RpcTarget.All, new object[] { playerID });
        }
    }

    [PunRPC]
    private void ConfirmPrompt(int playerViewID)
    {
        PhotonView playerView = PhotonView.Find(playerViewID);

        if (playerView)
        {
            playerView.gameObject.transform.Find("_Confirm").gameObject.SetActive(true);

            Variables.Object(this).Set("activateConfirmFor", null);
        }
    }

    [PunRPC]
    private void DenyPrompt(int playerViewID)
    {
        PhotonView playerView = PhotonView.Find(playerViewID);

        if (playerView.IsMine)
        {
            playerView.gameObject.transform.Find("_Deny").gameObject.SetActive(true);

            Variables.Object(this).Set("activateDenyFor", null);
        }
    }

    [PunRPC]
    private void Kick(int playerViewID)
    {
        // Find the player object associated with the given view ID
        PhotonView playerView = PhotonView.Find(playerViewID);

        // Make sure we're not kicking ourselves
        if (playerView.IsMine)
        {
            // Kick the player from the room
            Debug.Log($"Player {kickedPlayerNickname} has been kicked from the room!");

            // Destroys the player object that is kicked, hopefully
            PhotonNetwork.Destroy(playerView.gameObject);

            // Load the kicked scene for this player only
            SceneManager.LoadScene("Kicked");
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CustomEvent.Trigger(this.gameObject, "PlayerLeft", otherPlayer.NickName);
    }
}