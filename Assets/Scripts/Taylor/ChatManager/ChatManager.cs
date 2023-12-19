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

    public List<string> colorCommands = new List<string>();

    public string newGuy1;

    private void Start()
    {
        chatterSpawner = FindObjectOfType<ChatterSpawner>();
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

            //ignore this, gonna look into why I have it here later
            newGuy1 = chatter;
        }
    }
}
