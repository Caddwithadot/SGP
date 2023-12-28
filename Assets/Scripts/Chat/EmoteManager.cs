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
    }

    // Define a wrapper class to represent the structure of the entire response
    [System.Serializable]
    public class EmoteResponse
    {
        public Emote[] emotes;
    }

    // Dictionary to store emotes with code as the key
    private Dictionary<string, Emote> emoteDictionary = new Dictionary<string, Emote>();

    private Image image;
    public string emoteTest;

    void Awake()
    {
        StartCoroutine(Test());
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (emoteDictionary.ContainsKey(emoteTest))
        {
            if (image.material.mainTexture != emoteDictionary[emoteTest].texture)
            {
                // Destroy the current Image component
                Destroy(image);

                // Add a new Image component
                Image newImage = gameObject.AddComponent<Image>();

                // Set the material texture
                newImage.material.mainTexture = emoteDictionary[emoteTest].texture;

                // Assign the new Image component to the 'image' variable
                image = newImage;

                Debug.Log("Changed texture");
            }
        }
    }

    IEnumerator Test()
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
            EmoteResponse response = JsonUtility.FromJson<EmoteResponse>("{\"emotes\":" + www.downloadHandler.text + "}");

            // Access the array within the response
            Emote[] emotes = response.emotes;

            // Iterate through the array and print id and code for each item
            foreach (Emote emote in emotes)
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
                    emoteDictionary[emote.code] = emote;
                }
            }

            // Print every dictionary item
            foreach (var kvp in emoteDictionary)
            {
                Debug.Log("Key: " + kvp.Key + ", Value (ID): " + kvp.Value.id + ", Value (Texture): " + kvp.Value.texture);
            }
        }
    }
}