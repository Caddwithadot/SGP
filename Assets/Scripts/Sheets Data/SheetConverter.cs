using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SheetConverter : MonoBehaviour
{
    [SerializeField]
    private string lastLetter;

    [SerializeField]
    private int maxNum = 1000;

    private SheetWriter sheetWriter;
    private SheetReader sheetReader;
    private SheetData sheetData;
    private int lastRow;

    private GameObject chatManager;

    public void Start()
    {
        sheetWriter = new SheetWriter();
        sheetReader = new SheetReader();
        sheetData = sheetReader.GetSheetData("Sheet1!A1:" + lastLetter + maxNum);
        lastRow = sheetReader.GetLastRow();

        chatManager = GameObject.FindGameObjectWithTag("ChatManager");

        Debug.Log("Last row: " + lastRow);
    }

    void Update()
    {
        if(Variables.Object(chatManager).Get<string>("newGuy1") != null)
        {
            string nameCheck = Variables.Object(chatManager).Get<string>("newGuy1");

            ReadTheShit(nameCheck);

            Variables.Object(chatManager).Set("newGuy1", null);
        }
    }

    void ReadTheShit(string name)
    {
        bool nameExists = false;

        IList<IList<object>> values = sheetData.values;

        foreach (var row in values)
        {
            //checks all names in column A
            if (row[0].ToString() == name)
            {
                nameExists = true;
                break;
            }
        }

        if (!nameExists)
        {
            WriteTheShit(name);
        }

        Variables.Object(gameObject).Set("Rows", sheetData.numRows);
        Variables.Object(gameObject).Set("Columns", sheetData.numColumns);
    }

    void WriteTheShit(string name)
    {
        var writeValues = new List<IList<object>>();
        var newRow = new List<object>();

        newRow.Add(name);
        writeValues.Add(newRow);

        int rowIndex = lastRow + 1;

        sheetWriter.WriteData("Sheet1", "A" + rowIndex + ":" + lastLetter + rowIndex, writeValues);

        lastRow = rowIndex;

        Variables.Object(gameObject).Set("Rows", sheetData.numRows);
    }
}