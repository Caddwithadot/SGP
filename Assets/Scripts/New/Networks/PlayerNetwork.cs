using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerNetwork : MonoBehaviour
{
    public static PlayerNetwork Instance;
    private PhotonView photonView;
    private int PlayersInGame = 0;
    public GameObject PlayerPrefab;

    private void Awake()
    {
        Instance = this;

        photonView = GetComponent<PhotonView>();

        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            //photonView.RPC("RPC_CreatePlayer", RpcTarget.All);

            /*
            if (PhotonNetwork.IsMasterClient)
            {
                MasterLoadedGame();
            }
            else
            {
                NonMasterLoadedGame();
            }
            */
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

            photonView.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        Debug.Log("Spawning player object");
        Vector3 spawnPoint = new Vector3(0, 3.15f, 0);
        PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPoint, Quaternion.identity);

    }
}
