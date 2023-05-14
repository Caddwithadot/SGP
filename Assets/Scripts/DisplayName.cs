using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class DisplayName : MonoBehaviourPunCallbacks
{
    void Start()
    {
        GetComponent<TMP_Text>().text = photonView.Owner.NickName;
    }
}
