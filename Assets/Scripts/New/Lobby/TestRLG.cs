using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class TestRLG : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    [SerializeField]
    private GameObject _roomListingPrefab;
    private GameObject RoomListingPrefab
    {
        get { return _roomListingPrefab; }
    }

    private List<RoomListing> _roomListingButtons = new List<RoomListing>();
    private List<RoomListing> RoomListingButtons
    {
        get { return _roomListingButtons; }
    }

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

    public void RemoveRoom(RoomInfo room)
    {
        int index = RoomListingButtons.FindIndex(x => x.RoomName == room.Name);
        if (index != -1)
        {
            RoomListing roomListing = RoomListingButtons[index];
            RoomListingButtons.RemoveAt(index);
            Destroy(roomListing.gameObject);
        }
    }

    private void UpdateOrCreateRoom(RoomInfo room)
    {
        int index = RoomListingButtons.FindIndex(x => x.RoomName == room.Name);

        if (index == -1)
        {
            if (room.IsVisible && room.PlayerCount < room.MaxPlayers)
            {
                GameObject roomListingObj = Instantiate(RoomListingPrefab);
                roomListingObj.transform.SetParent(transform, false);

                RoomListing roomListing = roomListingObj.GetComponent<RoomListing>();
                roomListing.SetRoomNameText(room.Name);

                //////////
                roomListing.SetRoomCountText(room.PlayerCount);

                RoomListingButtons.Add(roomListing);
            }
        }
        else
        {
            RoomListing roomListing = RoomListingButtons[index];
            roomListing.SetRoomNameText(room.Name);

            /////////
            roomListing.SetRoomCountText(room.PlayerCount);
        }
    }
}