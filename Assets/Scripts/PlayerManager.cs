using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CustomEvent.Trigger(this.gameObject, "PlayerLeft", otherPlayer.NickName);
    }
}