using Photon.Pun;
using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour
{
    public void OnClickStartSync()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        PhotonNetwork.LoadLevel("Game");
    }

    public void OnClickStartDelayed()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel("Game");
    }

    public void OnClickStart()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
