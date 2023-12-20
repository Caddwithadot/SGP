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
    private ChatterSpawner chatterSpawner;
    private FinalConverter finalConverter;

    public List<string> colorCommands = new List<string>();
    public List<ChatDisplayCanvas> chatDisplays = new List<ChatDisplayCanvas>();

    private void Start()
    {
        chatterSpawner = FindObjectOfType<ChatterSpawner>();
        finalConverter = FindObjectOfType<FinalConverter>();

        UpdateChatDisplayList();
    }

    public void NewMessage(string chatter, string message)
    {
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
        }

        UpdateChatDisplayList();
    }

    public void UpdateChatDisplayList()
    {
        chatDisplays.Clear();
        ChatDisplayCanvas[] chatDisplayObjects = FindObjectsOfType<ChatDisplayCanvas>();
        chatDisplays.AddRange(chatDisplayObjects);
    }
}
