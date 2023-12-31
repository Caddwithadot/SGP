/*******************************************************************************
Author: Taylor
State: Currently working on
Description:
Retrieves all emotes on awake from Twitch and BetterTTV and assigns them to a dictionary.
*******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [System.Serializable]
    public class UserResponse
    {
        public string id;
        public string[] bots;
        public string avatar;
        public Emote[] channelEmotes;
        public Emote[] sharedEmotes;
    }

    // Dictionary to store emotes with code as the key
    public Dictionary<string, Emote> staticEmoteDictionary = new Dictionary<string, Emote>();

    //default is possilbycadd
    //go here to find different ID https://www.streamweasels.com/tools/convert-twitch-username-to-user-id/
    public string channelID = "587142171";

    void Awake()
    {
        StartCoroutine(ActiveBetterTTV());
        StartCoroutine(RetrieveBetterTTVGlobal());
    }

    //retrieves currently used BetterTTV emotes for the channel
    IEnumerator ActiveBetterTTV()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://api.betterttv.net/3/cached/users/twitch/" + channelID);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            // Deserialize the JSON response
            UserResponse userResponse = JsonUtility.FromJson<UserResponse>(www.downloadHandler.text);

            // Access the array of emotes
            Emote[] emotes = userResponse.sharedEmotes;

            // Iterate through the emotes and do something with them
            foreach (Emote emote in emotes)
            {
                //handles static emotes
                if (emote.imageType == "png")
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
        }
    }

    //retrieves all static global BetterTTV emotes
    IEnumerator RetrieveBetterTTVGlobal()
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
                //handles static emotes
                if (emote.imageType == "png")
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

    // retrieves all pre-downloaded global Twitch emotes
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
            Debug.Log(kvp.Key);
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