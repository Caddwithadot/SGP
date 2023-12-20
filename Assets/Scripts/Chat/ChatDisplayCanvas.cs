/*******************************************************************************
Author: Jared
State: Complete/Functional
Description:
 Allows the player to look around using the mouse.
*******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatDisplayCanvas : MonoBehaviour
{
    private ChatManager chatManager;

    public GameObject ChatBoxParent;
    public GameObject ChatBoxPrefab;

    private string Chatter;
    private string Message;

    void Start()
    {
        chatManager = FindObjectOfType<ChatManager>();
    }

    public void DisplayNewMessage(string chatter, string message)
    {
        GameObject chatBox = Instantiate(ChatBoxPrefab, ChatBoxParent.transform);

        TextMeshPro Text = chatBox.GetComponentInChildren<TextMeshPro>();

        Text.text = (chatter + ": " + message);
        //Text.color = 
    }
}
