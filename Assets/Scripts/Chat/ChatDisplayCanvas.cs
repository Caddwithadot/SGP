/*******************************************************************************
Author: Jared
State: In progress of adding emotes (Needs comments)
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
    private EmoteManager emoteManager;

    public GameObject ChatBoxParent;
    public GameObject ChatBoxPrefab;
    public TextMeshProUGUI HeaderLabel;

    public ScrollRect scrollRect;

    void Start()
    {
        chatManager = FindObjectOfType<ChatManager>();
        twitchConnect = FindObjectOfType<TwitchConnect>();
        emoteManager = FindObjectOfType<EmoteManager>();

        //Sets the header label to the current streamer's name
        CurrentChat = twitchConnect.Channel;
        HeaderLabel.text = "LIVE " + CurrentChat.ToUpper() + " TWITCH CHAT";
    }

    public void DisplayNewMessage(string chatter, string message)
    {
        //MOVE THIS to start or make a function
        List<string> EmoteNames = new List<string>(emoteManager.staticEmoteDictionary.Keys);

        for (int i = 0; i < emoteManager.staticEmoteDictionary.Count; i++)
        {
            if (message.Contains(EmoteNames[i]))
            {
                message = message.Replace(EmoteNames[i], "<sprite name=" + emoteManager.staticEmoteDictionary[EmoteNames[i]].code + ">");

                Debug.Log(message);
            }
        }

        string EmotedMessage = message;

        //Gets a reference to the instantiated chat box
        GameObject chatBox = Instantiate(ChatBoxPrefab, ChatBoxParent.transform);

        //Changes chat box image appearance
        Color boxColor = new Color(Random.value, Random.value, Random.value);
        chatBox.GetComponent<Image>().color = boxColor;

        //Changes chat box text
        TextMeshProUGUI Text = chatBox.GetComponentInChildren<TextMeshProUGUI>();
        Text.text = (chatter + ": " + EmotedMessage);

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
