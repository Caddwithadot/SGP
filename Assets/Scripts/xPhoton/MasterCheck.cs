using Photon.Pun;
using UnityEngine;

public class MasterCheck : MonoBehaviour
{
    public GameObject TwitchConnect;
    public GameObject ChatManager;
    public GameObject ChatterManager;
    public GameObject DataManager;

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Destroy(TwitchConnect);
            Destroy(ChatManager);
            Destroy(ChatterManager);
            Destroy(DataManager);
        }
    }
}
