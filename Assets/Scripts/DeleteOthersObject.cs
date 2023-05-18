using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DeleteOthersObject : MonoBehaviour
{
    private void Awake()
    {
        PhotonView thisView = GetComponent<PhotonView>();

        if (!thisView.IsMine)
        {

        }
    }
}