using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayoutGroup : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject playerListingPrefab;

    private List<PlayerListing> playerListings = new List<PlayerListing>();

    //Use this only if we want to cick out all players when the master client leaves.
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedRoom()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        MainCanvasManager.Instance.CurrentRoomCanvas.transform.SetAsLastSibling();

        Photon.Realtime.Player[] photonPlayers = PhotonNetwork.PlayerList;
        foreach (Photon.Realtime.Player photonPlayer in photonPlayers)
        {
            PlayerJoinedRoom(photonPlayer);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        PlayerJoinedRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        PlayerLeftRoom(otherPlayer);
    }

    private void PlayerJoinedRoom(Photon.Realtime.Player photonPlayer)
    {
        if (photonPlayer == null)
        {
            return;
        }

        PlayerLeftRoom(photonPlayer);

        GameObject playerListingObj = Instantiate(playerListingPrefab);
        playerListingObj.transform.SetParent(transform, false);

        PlayerListing playerListing = playerListingObj.GetComponent<PlayerListing>();
        playerListing.ApplyPhotonPlayer(photonPlayer);

        playerListings.Add(playerListing);
    }

    private void PlayerLeftRoom(Photon.Realtime.Player photonPlayer)
    {
        int index = playerListings.FindIndex(x => x.PhotonPlayer == photonPlayer);
        if (index != -1)
        {
            Destroy(playerListings[index].gameObject);
            playerListings.RemoveAt(index);
        }
    }

    public void OnClickRoomState()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        PhotonNetwork.CurrentRoom.IsOpen = !PhotonNetwork.CurrentRoom.IsOpen;
        PhotonNetwork.CurrentRoom.IsVisible = PhotonNetwork.CurrentRoom.IsOpen;
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}