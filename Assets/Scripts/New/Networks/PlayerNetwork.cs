using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerNetwork : MonoBehaviour
{
    public static PlayerNetwork Instance;
    private PhotonView photonView;
    private int PlayersInGame = 0;

    private void Awake()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();

        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        // If commented, AutomaticallySync in PlayerNetwork.cs should be TRUE

        if (scene.name == "Game")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                MasterLoadedGame();
            }
            else
            {
                NonMasterLoadedGame();
            }
        }
    }

    private void MasterLoadedGame()
    {
        photonView.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
        photonView.RPC("RPC_LoadGameOthers", RpcTarget.Others);
    }

    private void NonMasterLoadedGame()
    {
        photonView.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void RPC_LoadGameOthers()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        PlayersInGame++;
        if(PlayersInGame == PhotonNetwork.PlayerList.Length)
        {
            print("All players in the game scene.");
        }
    }
}
