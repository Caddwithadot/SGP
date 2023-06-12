using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class TestRLG : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    [SerializeField]
    private GameObject roomListingPrefab;

    private List<RoomListing> roomListingButtons = new List<RoomListing>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            RoomReceived(room);
        }
    }

    private void RoomReceived(RoomInfo room)
    {
        if (room.RemovedFromList)
        {
            RemoveRoom(room);
        }
        else
        {
            UpdateOrCreateRoom(room);
        }
    }

    private void RemoveRoom(RoomInfo room)
    {
        int index = roomListingButtons.FindIndex(x => x.RoomName == room.Name);
        if (index != -1)
        {
            RoomListing roomListing = roomListingButtons[index];
            roomListingButtons.RemoveAt(index);
            Destroy(roomListing.gameObject);
        }
    }

    private void UpdateOrCreateRoom(RoomInfo room)
    {
        int index = roomListingButtons.FindIndex(x => x.RoomName == room.Name);

        if (index == -1)
        {
            if (room.IsVisible && room.PlayerCount < room.MaxPlayers)
            {
                GameObject roomListingObj = Instantiate(roomListingPrefab);
                roomListingObj.transform.SetParent(transform, false);

                RoomListing roomListing = roomListingObj.GetComponent<RoomListing>();
                roomListing.SetRoomNameText(room.Name);
                roomListingButtons.Add(roomListing);
            }
        }
        else
        {
            RoomListing roomListing = roomListingButtons[index];
            roomListing.SetRoomNameText(room.Name);
        }
    }
}