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

    private void Start()
    {
        //PhotonNetwork.IsMessageQueueRunning = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            int playerID = GameObject.Find(kickedPlayerNickname).GetComponent<PhotonView>().ViewID;

            GetComponent<PhotonView>().RPC("Kick", RpcTarget.Others, new object[] { playerID });
        }
        /*
        if (Variables.Object(this).Get<int>("activateConfirmFor") > 0)
        {
            int playerID = Variables.Object(this).Get<int>("activateConfirmFor");

            GetComponent<PhotonView>().RPC("ConfirmPrompt", RpcTarget.All, new object[] { playerID });
        }

        if (Variables.Object(this).Get<int>("activateDenyFor") > 0)
        {
            int playerID = Variables.Object(this).Get<int>("activateDenyFor");

            GetComponent<PhotonView>().RPC("Deny", RpcTarget.Others, new object[] { playerID });
        }
        */
    }

    [PunRPC]
    private void ConfirmPrompt(int playerViewID)
    {
        PhotonView playerView = PhotonView.Find(playerViewID);

        if (playerView.IsMine)
        {
            Debug.Log("Confirm: " + playerView);
            playerView.gameObject.transform.Find("_Confirm").gameObject.transform.Find("ConfirmPrompt").gameObject.SetActive(true);

            Variables.Object(playerView.gameObject).Set("currentlyConfirming", true);
        }
    }

    [PunRPC]
    private void Deny(int playerViewID)
    {
        PhotonView playerView = PhotonView.Find(playerViewID);

        if (playerView.IsMine)
        {
            PhotonNetwork.Destroy(playerView.gameObject);

            SceneManager.LoadScene("Lobby 1");

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
        int photonViewID = GetPhotonViewID(otherPlayer);
        CustomEvent.Trigger(this.gameObject, "PlayerLeft", otherPlayer.NickName, photonViewID);
    }

    private int GetPhotonViewID(Player player)
    {
        int photonViewID = -1;

        foreach (PhotonView photonView in PhotonNetwork.PhotonViewCollection)
        {
            if (photonView.Owner == player)
            {
                photonViewID = photonView.ViewID;
                break;
            }
        }

        return photonViewID;
    }
}