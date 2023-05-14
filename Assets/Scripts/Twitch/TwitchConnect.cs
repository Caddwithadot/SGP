using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class TwitchConnect : MonoBehaviour
{
    TcpClient Twitch;
    StreamReader Reader;
    StreamWriter Writer;

    public GameObject ChatManager;

    const string URL = "irc.chat.twitch.tv";
    const int PORT = 6667;

    string User = "sgp_alt";
    string OAuth = "oauth:vh0dxqt8dp6y58x9rg8aaa5oe5y21u";
    string Channel = "possiblycadd";

    float PingCounter = 0;

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

    private void Awake()
    {
        ConnectToTwitch();
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

                CustomEvent.Trigger(ChatManager, "NewMessage", chatter, msg);

                if (Variables.Object(ChatManager).Get<bool>("sendJoinNum"))
                {
                    SendJoinNum();
                    Variables.Object(ChatManager).Set("sendJoinNum", false);
                }
            }

            if(message.Contains("PING :tmi.twitch.tv"))
            {
                Writer.WriteLine("PONG " + "tmi.twitch.tv" + "\r\n");
                Writer.Flush();
                print("PONG");
                Debug.Log("PONG");
            }
        }
    }

    public void SendJoinNum()
    {
        Writer.WriteLine("PRIVMSG #" + Channel.ToLower() + " :" + "https://notcadd.itch.io/testing-game");
        Writer.Flush();
    }
}
