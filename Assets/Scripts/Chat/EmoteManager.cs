/*******************************************************************************
Author: Taylor
State: Currently working on
Description:
Retrieves all emotes on awake from Twitch and BetterTTV and assigns them to a dictionary.
*******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EmoteManager : MonoBehaviour
{
    // Define a class to represent each emote
    [System.Serializable]
    public class Emote
    {
        public string id;
        public string code;
        public Texture2D texture;
        public string imageType;
    }

    // Define a wrapper class to represent the structure of the entire response
    [System.Serializable]
    public class BetterTTVEmoteResponse
    {
        public Emote[] emotes;
    }

    // Dictionary to store emotes with code as the key
    public Dictionary<string, Emote> staticEmoteDictionary = new Dictionary<string, Emote>();

    void Awake()
    {
        StartCoroutine(RetrieveBetterTTV());
    }

    //retrieves all static BetterTTV emotes and adds them to the dictionary
    IEnumerator RetrieveBetterTTV()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://api.betterttv.net/3/cached/emotes/global");
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            // Deserialize the entire response into the EmoteResponse class
            BetterTTVEmoteResponse response = JsonUtility.FromJson<BetterTTVEmoteResponse>("{\"emotes\":" + www.downloadHandler.text + "}");

            // Access the array within the response
            Emote[] emotes = response.emotes;

            // Iterate through the array and print id and code for each item
            foreach (Emote emote in emotes)
            {
                if (emote.imageType != "gif")
                {
                    // Create the URL for the emote texture
                    string emoteTextureURL = "https://cdn.betterttv.net/emote/" + emote.id + "/3x";

                    // Fetch the emote texture using UnityWebRequestTexture
                    UnityWebRequest wwwTexture = UnityWebRequestTexture.GetTexture(emoteTextureURL);
                    yield return wwwTexture.SendWebRequest();

                    // Check for errors in the texture request
                    if (wwwTexture.result == UnityWebRequest.Result.ConnectionError ||
                        wwwTexture.result == UnityWebRequest.Result.ProtocolError)
                    {
                        Debug.LogError("Error fetching emote texture: " + wwwTexture.error);
                    }
                    else
                    {
                        // Set the texture in the emote object
                        emote.texture = ((DownloadHandlerTexture)wwwTexture.downloadHandler).texture;

                        // Add the emote to the dictionary
                        staticEmoteDictionary[emote.code] = emote;
                    }
                }
            }

            RetrieveTwitchGlobal();
        }
    }

    // retrieves pre-downloaded Twitch global emotes and adds them to the dictionary
    void RetrieveTwitchGlobal()
    {
        // Load all Texture2D objects from the specified folder in the "Resources" directory
        Texture2D[] emoteTextures = Resources.LoadAll<Texture2D>("Emotes/TwitchGlobal");

        // Iterate through the loaded textures and add them to the dictionary
        foreach (Texture2D texture in emoteTextures)
        {
            // Replace characters in the name if needed
            string emoteName = TwitchCharReplacement(texture.name);

            // Create a default Emote object for Twitch global emotes
            Emote twitchEmote = new Emote
            {
                id = "null",
                code = emoteName,
                texture = texture,
                imageType = "png"
            };

            // Add to the dictionary
            staticEmoteDictionary[emoteName] = twitchEmote;
        }

        // Print every dictionary item
        foreach (var kvp in staticEmoteDictionary)
        {
            Debug.Log("Key (ID): " + kvp.Key);
        }
    }

    //fixes twitch global names before adding them to the dictionary
    string TwitchCharReplacement(string input)
    {
        input = input.Replace('!', ':');
        input = input.Replace('@', '\\');
        input = input.Replace('#', '|');
        input = input.Replace('$', '<');
        input = input.Replace('%', '>');

        return input;
    }
}