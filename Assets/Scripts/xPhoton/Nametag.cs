/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class Nametag : MonoBehaviourPunCallbacks
{
    void Start()
    {
        gameObject.name = photonView.Owner.NickName;

        CustomEvent.Trigger(GameObject.FindGameObjectWithTag("PlayerManager"), "NewPlayer", gameObject.name, photonView.ViewID);
    }
}
*/