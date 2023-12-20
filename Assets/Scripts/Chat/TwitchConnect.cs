/*******************************************************************************
Author: Taylor
State: Complete
Description:
Recieves all information from specified Twitch chat.
*******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;

public class TwitchConnect : MonoBehaviour
{
    TcpClient Twitch;
    StreamReader Reader;
    StreamWriter Writer;

    private ChatManager chatManager;

    const string URL = "irc.chat.twitch.tv";
    const int PORT = 6667;

    string User = "sgp_alt";
    public string Channel;

    [SerializeField]
    private string OAuth;

    float PingCounter = 0;

    private void Awake()
    {
        chatManager = FindObjectOfType<ChatManager>();
        ConnectToTwitch();
    }

    private void ConnectToTwitch()
    {
        Twitch = new TcpClient(URL, PORT);
        Reader = new StreamReader(Twitch.GetStream());
        Writer = new StreamWriter(Twitch.GetStream());

        Writer.WriteLine("PASS " + OAuth);
        Writer.WriteLine("NICK " + User.ToLower());
        Writer.WriteLine("JOIN #" + Channel.ToLower());
        Writer.Flush();

        print("Connected to " + Channel + " 's chat.");
    }

    void Update()
    {
        PingCounter += Time.deltaTime;
        if(PingCounter > 60)
        {
            Writer.WriteLine("PING " + URL);
            Writer.Flush();
            PingCounter = 0;
        }

        if (!Twitch.Connected)
        {
            print("not connected");
            ConnectToTwitch();
        }

        if(Twitch.Available > 0)
        {
            string message = Reader.ReadLine();

            if (message.Contains("PRIVMSG"))
            {
                // :sgp_alt!sgp_alt@sgp_alt.tmi.twitch.tv PRIVMSG #possiblycadd :test test
                int splitPoint = message.IndexOf("!");
                string chatter = message.Substring(1, splitPoint - 1);

                splitPoint = message.IndexOf(":", 1);
                string msg = message.Substring(splitPoint + 1);

                //sends the information to the chatManager
                chatManager.NewMessage(chatter, msg);
            }

            if(message.Contains("PING :tmi.twitch.tv"))
            {
                Writer.WriteLine("PONG " + "tmi.twitch.tv" + "\r\n");
                Writer.Flush();
            }
        }
    }
}
