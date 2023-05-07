using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVWriter : MonoBehaviour
{
    string filename = "";

    [System.Serializable]
    public class Player
    {
        public string Chatter;
        public string Display;
    }
    [System.Serializable]
    public class PlayerList
    {
        public Player[] player;
    }

    public PlayerList myPlayerList = new PlayerList();

    void Start()
    {
        filename = Application.dataPath + "/SGP_PlayerData.csv";
    }

    public void WriteCSV()
    {
        if(myPlayerList.player.Length > 0)
        {
            TextWriter tw = new StreamWriter(filename, false);
            tw.WriteLine("Chatter, Display");
            tw.Close();

            tw = new StreamWriter(filename, true);
            for(int i = 0; i < myPlayerList.player.Length; i++)
            {
                tw.WriteLine(myPlayerList.player[i].Chatter + "," + myPlayerList.player[i].Display);
            }
            tw.Close();
        }
    }
}
