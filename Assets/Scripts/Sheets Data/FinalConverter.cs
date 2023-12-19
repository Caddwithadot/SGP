using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FinalConverter : MonoBehaviour
{
    public int maxNum = 1000;
    private int lastRow;

    private SheetWriter sheetWriter;
    private SheetReader sheetReader;
    private SheetData sheetData;

    public List<string> nameList = new List<string>();
    public List<string> colorList = new List<string>();

    private string nameOfChanger;
    private string colorChange;


    public void Start()
    {
        sheetWriter = new SheetWriter();
        sheetReader = new SheetReader();

        sheetData = sheetReader.GetSheetData("Sheet1!A2:B" + maxNum);
        lastRow = sheetReader.GetLastRow();

        IList<IList<object>> values = sheetData.values;

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
        if (nameOfChanger != null)
        {
            string name = nameOfChanger;

            if (colorChange != null)
            {
                string color = colorChange;

                UpdateColor(name, color);

                colorChange = null;
            }

            nameOfChanger = null;
        }

        /* need to implement some way to automatically do this when closing the application
        if (Input.GetKeyDown(KeyCode.F))
        {
            WriteShit(nameList, colorList);
        }
        */
    }

    public void CheckName(string name)
    {
        if (!nameList.Contains(name))
        {
            GameObject foundObject = GameObject.Find(name);

            if (foundObject != null)
            {
                Chatter chatterComponent = foundObject.GetComponent<Chatter>();
                if (chatterComponent != null)
                {
                    nameList.Add(name);
                    colorList.Add(chatterComponent.colorNum.ToString());
                }
            }
        }
        else
        {
            int index = nameList.IndexOf(name);

            if (index != -1)
            {
                string color = colorList[index];
                GameObject foundObject = GameObject.Find(name);

                if (foundObject != null)
                {
                    Chatter chatterComponent = foundObject.GetComponent<Chatter>();
                    if (chatterComponent != null)
                    {
                        chatterComponent.colorNum = int.Parse(color);
                    }
                }
            }
        }
    }

    void UpdateColor(string name, string color)
    {
        int index = nameList.IndexOf(name);

        if (index != -1)
        {
            colorList[index] = color;
        }
    }

    void WriteShit(List<string> names, List<string> colors)
    {
        var writeNameValues = new List<IList<object>>();
        var writeColorValues = new List<IList<object>>();

        foreach (string name in names)
        {
            var nameRow = new List<object>();
            nameRow.Add(name);
            writeNameValues.Add(nameRow);
        }

        foreach (string color in colors)
        {
            var colorRow = new List<object>();
            colorRow.Add(color);
            writeColorValues.Add(colorRow);
        }

        sheetWriter.WriteData("Sheet1", "A2:A" + maxNum, writeNameValues);
        sheetWriter.WriteData("Sheet1", "B2:B" + maxNum, writeColorValues);
    }
}