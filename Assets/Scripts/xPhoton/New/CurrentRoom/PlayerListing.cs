using UnityEngine;
using Photon.Realtime;
using TMPro;

public class PlayerListing : MonoBehaviour
{
    public Player PhotonPlayer { get; private set; }

    [SerializeField]
    private TMP_Text _playerName;
    private TMP_Text PlayerName
    {
        get { return _playerName; }
    }

    public void ApplyPhotonPlayer(Player photonPlayer)
    {
        PhotonPlayer = photonPlayer;
        PlayerName.text = photonPlayer.NickName;
    }
}
