/*******************************************************************************
Author: Taylor
State: Working, need to get an autosave set up.
Description:
Handles reading and writing to the spreadsheet.
*******************************************************************************/

using System.Collections.Generic;
using UnityEngine;

public class FinalConverter : MonoBehaviour
{
    private SheetWriter sheetWriter;
    private SheetReader sheetReader;
    private SheetData sheetData;

    private ChatterSpawner chatterSpawner;

    public int maxNum = 1000;

    //every name, including ones that have already been written.
    public List<string> nameList = new List<string>();
    public List<string> colorList = new List<string>();

    private void Awake()
    {
        chatterSpawner = FindObjectOfType<ChatterSpawner>();

        sheetWriter = new SheetWriter();
        sheetReader = new SheetReader();

        //gets all data from columns 'A' and 'B' in the SheetReader.cs
        sheetData = sheetReader.GetSheetData("Sheet1!A2:B" + maxNum);
        IList<IList<object>> values = sheetData.values;

        //goes through every row to add all pre-existing names and colors to the lists
        foreach (var row in values)
        {
            if (row.Count > 0)
            {
                string name = row[0].ToString();
                nameList.Add(name);

                string color = row[1].ToString();
                colorList.Add(color);
            }
        }
    }

    void Update()
    {
        // need to implement some way to automatically do this before closing the game
        /*
        if (Input.GetKeyDown(KeyCode.F))
        {
            WriteShit(nameList, colorList);
        }
        */
    }

    //updates the saved color in the list of the given chatter
    public void UpdateColor(string name, string color)
    {
        int index = nameList.IndexOf(name);

        if (index != -1)
        {
            colorList[index] = color;
        }
    }

    //new chatter has been instantiated
    public void CheckName(string name)
    {
        //checks if this chatter has talked in a previous session
        if (!nameList.Contains(name))
        {
            //chatter is new
            if (chatterSpawner.chatterDictionary.ContainsKey(name))
            {
                Chatter chatterComponent = chatterSpawner.chatterDictionary[name].GetComponent<Chatter>();

                //adds the new chatter to get saved
                nameList.Add(name);
                colorList.Add(chatterComponent.colorNum.ToString());
            }
        }
        else
        {
            //gets the index of the pre-existing chatter
            int index = nameList.IndexOf(name);

            if (index != -1)
            {
                //gets the color they had previously saved
                string color = colorList[index];

                //checks again if the instantiated chatter exists
                if (chatterSpawner.chatterDictionary.ContainsKey(name))
                {
                    Chatter chatterComponent = chatterSpawner.chatterDictionary[name].GetComponent<Chatter>();

                    //sets the instantiated chatter's color to their corresponding saved color as they spawn
                    chatterComponent.colorNum = int.Parse(color);
                }
            }
        }
    }

    void WriteShit(List<string> names, List<string> colors)
    {
        var writeNameValues = new List<IList<object>>();
        var writeColorValues = new List<IList<object>>();

        //sets up name column for writing
        foreach (string name in names)
        {
            var nameRow = new List<object>();
            nameRow.Add(name);
            writeNameValues.Add(nameRow);
        }

        //sets up color column for writing
        foreach (string color in colors)
        {
            var colorRow = new List<object>();
            colorRow.Add(color);
            writeColorValues.Add(colorRow);
        }

        //saves all values by writing them in their designated columns
        sheetWriter.WriteData("Sheet1", "A2:A" + maxNum, writeNameValues);
        sheetWriter.WriteData("Sheet1", "B2:B" + maxNum, writeColorValues);
    }
}