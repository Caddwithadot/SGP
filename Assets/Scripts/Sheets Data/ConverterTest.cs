using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConverterTest : MonoBehaviour
{
    public string lastLetter;

    public int maxNum;

    private SheetWriter sheetWriter;
    private SheetReader sheetReader;
    private SheetData sheetData;
    private int lastRow;

    private GameObject chatManager;
    private List<string> nameList;
    private List<string> newNames;

    private void Awake()
    {
        chatManager = GameObject.FindGameObjectWithTag("ChatManager");
        nameList = new List<string>();
        newNames = new List<string>();
    }

    public void Start()
    {
        sheetWriter = new SheetWriter();
        sheetReader = new SheetReader();
        sheetData = sheetReader.GetSheetData("Sheet1!A1:" + lastLetter + maxNum);
        lastRow = sheetReader.GetLastRow();

        Debug.Log("Last row: " + lastRow);

        IList<IList<object>> values = sheetData.values;

        foreach (var row in values)
        {
            if (row.Count > 0)
            {
                string name = row[0].ToString();
                nameList.Add(name);
            }
        }

        newNames = GetNewNames(nameList);
    }

    void Update()
    {
        if (Variables.Object(chatManager).Get<string>("newGuy1") != null)
        {
            string name = Variables.Object(chatManager).Get<string>("newGuy1");

            ReadTheShit(name);

            Variables.Object(chatManager).Set("newGuy1", null);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            WriteTheShit(newNames);
        }
    }

    void ReadTheShit(string name)
    {
        if (!nameList.Contains(name) && !newNames.Contains(name))
        {
            newNames.Add(name);
        }

        Variables.Object(gameObject).Set("Rows", sheetData.numRows);
        Variables.Object(gameObject).Set("Columns", sheetData.numColumns);
    }

    List<string> GetNewNames(List<string> allNames)
    {
        List<string> newNames = new List<string>();

        foreach (string name in nameList)
        {
            if (!allNames.Contains(name))
            {
                newNames.Add(name);
            }
        }

        return newNames;
    }

    void WriteTheShit(List<string> names)
    {
        var writeValues = new List<IList<object>>();

        int remainingSpace = maxNum - lastRow;

        int namesToWrite = Mathf.Min(names.Count, remainingSpace);

        for (int i = 0; i < namesToWrite; i++)
        {
            string name = names[i];
            if (!nameList.Contains(name))
            {
                var newRow = new List<object>();
                newRow.Add(name);
                writeValues.Add(newRow);
                nameList.Add(name); // Add the name to nameList
            }
        }

        if (writeValues.Count > 0)
        {
            int startIndex = lastRow + 1;
            int endIndex = startIndex + writeValues.Count - 1;

            sheetWriter.WriteData("Sheet1", "A" + startIndex + ":" + lastLetter + endIndex, writeValues);

            lastRow = endIndex;
        }

        Variables.Object(gameObject).Set("Rows", sheetData.numRows);

        if (lastRow >= maxNum)
        {
            Debug.Log("MaxNum limit reached!");
        }

        newNames = GetNewNames(nameList); // Update newNames list after writing
    }
}