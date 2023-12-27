/*******************************************************************************
Author: Jared
State: In progress
Description:
 Displays messages in the chat canvas
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

        TextMeshProUGUI Text = chatBox.GetComponentInChildren<TextMeshProUGUI>();

        Text.text = (chatter + ": " + message);
        //Text.color = 
    }
}
