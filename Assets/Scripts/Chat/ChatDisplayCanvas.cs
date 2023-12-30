/*******************************************************************************
Author: Jared
State: In progress (Needs comments)
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
    private string CurrentChat = "STREAMER";

    private ChatManager chatManager;
    private TwitchConnect twitchConnect;

    public GameObject ChatBoxParent;
    public GameObject ChatBoxPrefab;
    public TextMeshProUGUI HeaderLabel;

    public ScrollRect scrollRect;

    void Start()
    {
        chatManager = FindObjectOfType<ChatManager>();
        twitchConnect = FindObjectOfType<TwitchConnect>();

        //Sets the header label to the current streamer's name
        CurrentChat = twitchConnect.Channel;
        HeaderLabel.text = "LIVE " + CurrentChat.ToUpper() + " TWITCH CHAT";
    }

    public void DisplayNewMessage(string chatter, string message)
    {
        //Gets a reference to the instantiated chat box
        GameObject chatBox = Instantiate(ChatBoxPrefab, ChatBoxParent.transform);

        //Changes chat box image appearance
        Color boxColor = new Color(Random.value, Random.value, Random.value);
        chatBox.GetComponent<Image>().color = boxColor;

        //Changes chat box text
        TextMeshProUGUI Text = chatBox.GetComponentInChildren<TextMeshProUGUI>();
        Text.text = (chatter + ": " + message);

        //Updates vertical layout group
        LayoutRebuilder.ForceRebuildLayoutImmediate(ChatBoxParent.GetComponent<RectTransform>());
    }

    public void ScrollToBottom()
    {
        //If it's not scrolled all the way down
        if (scrollRect.verticalNormalizedPosition == 0f)
        {
            //Scrolls down
            ForceScrollToBottom();
        }
    }

    //Separate function for the button
    public void ForceScrollToBottom()
    {
        //Scrolls to the bottom
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
