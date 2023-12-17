using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    private ChatterSpawner chatterSpawner;
    private ChatterManager chatterManager;

    public GameObject chatterPrefab;

    public string newGuy;
    public string newGuy1;

    public List<string> colorCommands = new List<string>();

    private void Start()
    {
        chatterManager = FindObjectOfType<ChatterManager>();
        chatterSpawner = FindObjectOfType<ChatterSpawner>();
    }

    public void NewMessage(string chatter, string message)
    {
        //checks if the chatter already exists
        if (chatterManager.chatterList.Contains(chatter))
        {
            //tells the chatter's object they talked in chat
            GameObject.Find(chatter).GetComponent<Chatter>().ChatterTalked(message);
        }
        else
        {
            //adds the new chatter to the list
            chatterManager.chatterList.Add(chatter);

            newGuy = chatter;
            newGuy1 = chatter;
        }

        /*
        // Check if the Chatter already exists in the dictionary
        if (chatterSpawner.chatterDictionary.ContainsKey(chatter))
        {
            // Tell the Chatter object they talked in chat
            chatterSpawner.chatterDictionary[chatter].ChatterTalked(message);
        }
        else
        {
            // Add the new Chatter to the dictionary
            chatterSpawner.chatterDictionary[chatter] = chatterSpawner.CreateChatterObject(chatter);

            newGuy1 = chatter;
        }
        */
    }
}
