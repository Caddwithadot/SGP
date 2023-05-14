using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public string targetPlayerNickname;
    Nametag nametag;

    private void Update()
    {
        // Check if the sendPlayerIdKey is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Search for the player with the targetPlayerNickname
            Photon.Realtime.Player targetPlayer = null;
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                if (player.NickName == targetPlayerNickname)
                {
                    targetPlayer = player;
                    break;
                }
            }

            // If the target player is found, send the local player's ID to the SceneLoader script
            if (targetPlayer != null)
            {
                nametag.GetKicked(targetPlayer.ToString());
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CustomEvent.Trigger(this.gameObject, "PlayerLeft", otherPlayer.NickName);
    }
}