/*******************************************************************************
Author: Taylor
State: Working, trying to figure out how to get more information from a chatter
Description:
Recieves all information from specified Twitch chat.
*******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;
using UnityEngine.Networking;

[System.Serializable]
public class BTTVEmoteInfo
{
    public string id;
    public string code;
    public string imageType;
}

public class TwitchConnect : MonoBehaviour
{
    TcpClient Twitch;
    StreamReader Reader;
    StreamWriter Writer;

    private ChatManager chatManager;

    string User = "sgp_alt";
    public string Channel;

    [SerializeField]
    private string OAuth;

    float PingCounter = 0;

    //emote test
    public GameObject emotePrefab;
    private HashSet<string> uniqueEmoteIDs = new HashSet<string>();
    public Transform emoteParent;

    private void Awake()
    {
        chatManager = FindObjectOfType<ChatManager>();
        ConnectToTwitch();
    }

    private void ConnectToTwitch()
    {
        Twitch = new TcpClient("irc.chat.twitch.tv", 6667);
        Reader = new StreamReader(Twitch.GetStream());
        Writer = new StreamWriter(Twitch.GetStream());

        Writer.WriteLine("PASS " + OAuth);

        //displayname is useless, it's the same as the username, just allows the user to change capital lettering
        Writer.WriteLine("NICK " + User.ToLower());

        //we want to track the username in the data sheet since their nickname can be changed at any time
        Writer.WriteLine("USER " + User.ToLower() + " 8 * :" + User.ToLower());

        //joining channel and requesting additional information
        Writer.WriteLine("JOIN #" + Channel.ToLower());
        Writer.WriteLine("CAP REQ :twitch.tv/tags");
        Writer.WriteLine("CAP REQ :twitch.tv/commands");
        Writer.WriteLine("CAP REQ :twitch.tv/membership");
        Writer.Flush();

        print("Connected to " + Channel + " 's chat.");
    }

    void Update()
    {
        PingCounter += Time.deltaTime;
        if (PingCounter > 60)
        {
            Writer.WriteLine("PING :tmi.twitch.tv");
            Writer.Flush();
            PingCounter = 0;
        }

        if (!Twitch.Connected)
        {
            print("not connected");
            ConnectToTwitch();
        }


        //UPDATE THIS STUFFS
        if (Twitch.Available > 0)
        {
            string message = Reader.ReadLine();

            //message from chatter
            if (message.Contains("PRIVMSG"))
            {
                //@badge - info =; badges = broadcaster / 1; client - nonce = 22a3689b9615bd8ece35d5896e123747; color =; display - name = PossiblyCadd; emotes =; first - msg = 0; flags =; id = da670411 - cac1 - 4518 - a786 - 01d698dff049; mod = 0; returning - chatter = 0; room - id = 587142171; subscriber = 0; tmi - sent - ts = 1703275843617; turbo = 0; user - id = 587142171; user - type = :possiblycadd!possiblycadd @possiblycadd.tmi.twitch.tv PRIVMSG #possiblycadd :test

                Debug.Log(message);

                //get the name 
                string[] stringSeparators = new string[] { "display-name=" };
                string[] result = message.Split(stringSeparators, StringSplitOptions.None);
                var splitPoint = result[1].IndexOf(";", 0);
                var chatter = result[1].Substring(0, splitPoint);

                //get the users message by splitting it from the string
                string[] stringSeparators2 = new string[] { "PRIVMSG" };
                string[] result2 = message.Split(stringSeparators2, StringSplitOptions.None);
                var splitPoint2 = result2[1].Split(':');
                string msg = splitPoint2[1];

                //output the username and chat message
                chatManager.NewMessage(chatter.ToLower(), msg);


                //EMOTE STUFFS
                // if the emotes list is not empty, get the emote texture
                if (!message.Contains("emotes=;"))
                {
                    string[] stringSeparatorsE = new string[] { "emotes=" };
                    string[] resultE = message.Split(stringSeparatorsE, StringSplitOptions.None);

                    // split the emote string in case of multiple emotes
                    var splitPointallEmotes = resultE[1].IndexOf(";", 0);
                    var allemotes = resultE[1].Substring(0, splitPointallEmotes);
                    string[] seperateEmotes = allemotes.Split('/');

                    // grab all emote textures
                    for (int i = 0; i < seperateEmotes.Length; i++)
                    {
                        var id = seperateEmotes[i].IndexOf(":", 0);
                        var emoteID = seperateEmotes[i].Substring(0, id);

                        // 1.0 / 2.0 / 3.0 is texture sizes
                        // light / dark is the theme mode
                        // static / animated is if the emote is animated or not

                        //global twitch
                        //StartCoroutine(GetTexture("https://static-cdn.jtvnw.net/emoticons/v2/" + emoteID + "/static" + "/light/3.0", emoteID));
                        //StartCoroutine(GetTexture("https://static-cdn.jtvnw.net/emoticons/v1/" + emoteID + "/3.0", emoteID));



                        //global betterttv
                        //StartCoroutine(GetBTTVEmoteInfo("https://cdn.betterttv.net/emote/" + emoteID + "/3x", emoteID));
                    }
                }

                //pongs the twitch chat back to prevent us from getting kicked from the chat
                if (message.Contains("PING :tmi.twitch.tv"))
                {
                    Writer.WriteLine("PONG " + "tmi.twitch.tv" + "\r\n");
                    Writer.Flush();
                }
            }
        }
    }

//getting the emotes texture
IEnumerator GetTexture(string url, string ID)
    {
        // find the emote texture
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        // set it to an image, and spawn a particle with that image
        Texture2D img = DownloadHandlerTexture.GetContent(www);

        //emote particle test
        //GameObject emotePart = Instantiate(emotePrefab, transform.position, transform.rotation);
        //emotePart.GetComponent<ParticleSystem>().GetComponent<Renderer>().material.mainTexture = img;

        //GameObject emotePart = Instantiate(emotePrefab, transform.position, transform.rotation, emoteParent);
        //emotePart.GetComponent<Image>().material.mainTexture = img;

        // save the texture to a folder
        //SaveTextureToFile(img, ID);
    }

    //saves texture to the EmoteTextures folder
    void SaveTextureToFile(Texture2D texture, string emoteID)
    {
        byte[] bytes = texture.EncodeToPNG();

        // Create a folder named "EmoteTextures" within the Assets folder
        string folderPath = Application.dataPath + "/Emotes";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Check if the emote ID is already saved
        if (!uniqueEmoteIDs.Contains(emoteID))
        {
            // Save the texture to a file in the folder
            File.WriteAllBytes(folderPath + "/" + emoteID + ".png", bytes);

            // Add the emote ID to the set of unique emote IDs
            uniqueEmoteIDs.Add(emoteID);
        }
    }
}