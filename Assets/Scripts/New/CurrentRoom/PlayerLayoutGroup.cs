using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerLayoutGroup : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject playerListingPrefab;

    private List<PlayerListing> playerListings = new List<PlayerListing>();

    public TestRLG testRLG;
    private Room prevRoom;

    public LobbyNetwork lobbyNetwork;

    //Use this only if we want to kick out all players when the master client leaves.
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        prevRoom = PhotonNetwork.CurrentRoom;
        prevRoom.IsOpen = false;
        prevRoom.IsVisible = false;

        lobbyNetwork.amRefreshing = true;
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
        lobbyNetwork.amRefreshing = true;
        PhotonNetwork.LeaveRoom();
    }
}