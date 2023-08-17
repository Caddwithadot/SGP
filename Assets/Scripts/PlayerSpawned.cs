using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using Photon.Chat;
using Photon.Pun;

public class PlayerSpawned : MonoBehaviour
{
    private GameObject ChatManager;

    void Awake()
    {
        ChatManager = GameObject.FindGameObjectWithTag("ChatManager");
    }

    private void Start()
    {
        CustomEvent.Trigger(ChatManager, "PlayerSpawned", this.name);
    }
}
