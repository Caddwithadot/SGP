/*******************************************************************************
Author: Taylor
State: Complete
Description:
Handles all messages coming from chat.
*******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    private FontManager fontManager;
    private ChatterSpawner chatterSpawner;
    private FinalConverter finalConverter;

    public List<string> colorCommands = new List<string>();
    public List<ChatDisplayCanvas> chatDisplays = new List<ChatDisplayCanvas>();

    private void Start()
    {
        fontManager = FindObjectOfType<FontManager>();
        chatterSpawner = FindObjectOfType<ChatterSpawner>();
        finalConverter = FindObjectOfType<FinalConverter>();

        UpdateChatDisplayList();
    }

    public void NewMessage(string chatter, string message)
    {
        //replace multiple spaces with a single space
        message = System.Text.RegularExpressions.Regex.Replace(message, @"\s+", " ");

        //checks if the chatter already exists
        if (chatterSpawner.chatterDictionary.ContainsKey(chatter))
        {
            //tells the chatter's object they talked in chat
            chatterSpawner.chatterDictionary[chatter].ChatterTalked(message);
        }
        else
        {
            //adds the new chatter to the list
            chatterSpawner.InstantiateNextChatter(chatter);
            finalConverter.CheckName(chatter);
        }

        foreach (ChatDisplayCanvas chatDisplay in chatDisplays)
        {
            chatDisplay.DisplayNewMessage(chatter, message);
            chatDisplay.ScrollToBottom();
        }

        UpdateChatDisplayList();

        //testing
        fontManager.InGameMessage(message);
    }

    public void UpdateChatDisplayList()
    {
        chatDisplays.Clear();
        ChatDisplayCanvas[] chatDisplayObjects = FindObjectsOfType<ChatDisplayCanvas>();
        chatDisplays.AddRange(chatDisplayObjects);
    }
}
