using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CSVReader : MonoBehaviour
{
    public TextAsset textAssetData;

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

    public void ReadCSV()
    {
        string[] data = textAssetData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);

        int tableSize = data.Length / 2 - 1;
        myPlayerList.player = new Player[tableSize];

        for(int i = 0; i < tableSize; i++)
        {
            myPlayerList.player[i] = new Player();
            myPlayerList.player[i].Chatter = data[2 * (i + 1)];
            myPlayerList.player[i].Display = data[2 * (i + 1) + 1];
        }
    }
}
