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

            Variables.Object(this).Set("activateConfirmFor", null);
        }

        if (Variables.Object(this).Get<string>("activateDenyFor") != null)
        {
            int playerID = Variables.Object(this).Get<int>("currentViewID");

            GetComponent<PhotonView>().RPC("DenyPrompt", RpcTarget.All, new object[] { playerID });

            Variables.Object(this).Set("activateDenyFor", null);
        }
    }

    [PunRPC]
    private void ConfirmPrompt(int playerViewID)
    {
        PhotonView playerView = PhotonView.Find(playerViewID);

        Debug.Log("1: " + playerView);
        if (playerView.IsMine)
        {
            Debug.Log("2: " + playerView);
            playerView.gameObject.transform.Find("_Confirm").gameObject.SetActive(true);

            Variables.Object(playerView.gameObject).Set("currentlyConfirming", true);
        }
    }

    [PunRPC]
    private void DenyPrompt(int playerViewID)
    {
        PhotonView playerView = PhotonView.Find(playerViewID);

        Debug.Log("3: " + playerView);
        if (playerView.IsMine)
        {
            if (Variables.Object(playerView.gameObject).Get<bool>("currentlyConfirming") == false)
            {
                Debug.Log("4: " + playerView);
                playerView.gameObject.transform.Find("_Deny").gameObject.SetActive(true);
            }
        }
    }

    [PunRPC]
    private void Kick(int playerViewID)
    {
        PhotonView playerView = PhotonView.Find(playerViewID);

        if (playerView.IsMine)
        {
            Debug.Log($"Player {kickedPlayerNickname} has been kicked from the room!");

            PhotonNetwork.Destroy(playerView.gameObject);

            SceneManager.LoadScene("Kicked");
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CustomEvent.Trigger(this.gameObject, "PlayerLeft", otherPlayer.NickName);
    }
}