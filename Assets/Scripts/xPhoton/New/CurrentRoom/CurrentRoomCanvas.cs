using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour
{
    /*
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
    */

    public void OnClickStart()
    {
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (player != null && player != PhotonNetwork.LocalPlayer)
            {
                if (player.NickName == PhotonNetwork.NickName)
                {
                    Debug.Log("Nickname '" + PhotonNetwork.NickName + "' is already taken.");
                    return;
                }
            }
        }

        Debug.Log("Nickname '" + PhotonNetwork.NickName + "' is available.");

        PhotonNetwork.LoadLevel("Game");
    }
}
