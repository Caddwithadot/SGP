using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class DisplayName : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject displayNamePrompt;
    public TMP_Text MyDisplayName;

    void Start()
    {
        PhotonNetwork.NickName = PlayerPrefs.GetString("DisplayName");

        MyDisplayName.text = PlayerPrefs.GetString("DisplayName");
    }

    public void SaveDisplayName()
    {
        PhotonNetwork.NickName = inputField.text;

        PlayerPrefs.SetString("DisplayName", inputField.text);

        MyDisplayName.text = inputField.text;
    }
}
