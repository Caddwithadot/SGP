using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerNetwork : MonoBehaviour
{
    public static PlayerNetwork Instance;
    private PhotonView photonView;

    private void Awake()
    {
        Instance = this;

        photonView = GetComponent<PhotonView>();
    }
}
