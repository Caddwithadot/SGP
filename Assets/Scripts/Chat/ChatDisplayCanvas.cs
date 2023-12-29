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
using UnityEngine.UI;

public class ChatDisplayCanvas : MonoBehaviour
{
    private ChatManager chatManager;

    public GameObject ChatBoxParent;
    public GameObject ChatBoxPrefab;

    private string Chatter;
    private string Message;

    public ScrollRect scrollRect;

    void Start()
    {
        chatManager = FindObjectOfType<ChatManager>();

        
    }

    private void Update()
    {
        
    }

    public void DisplayNewMessage(string chatter, string message)
    {
        GameObject chatBox = Instantiate(ChatBoxPrefab, ChatBoxParent.transform);

        TextMeshProUGUI Text = chatBox.GetComponentInChildren<TextMeshProUGUI>();

        Text.text = (chatter + ": " + message);
        //Text.color = 
    }

    public void ScrollToBottom()
    {
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
