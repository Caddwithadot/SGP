using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SheetConverter : MonoBehaviour
{
    public string lastLetter;

    public int maxNum;

    private SheetWriter sheetWriter;
    private SheetReader sheetReader;
    private SheetData sheetData;
    private int lastRow;

    private GameObject chatManager;
    private List<string> nameList;
    private List<string> colorList;

    private List<string> newNames;

    private void Awake()
    {
        // Find the ChatManager GameObject
        chatManager = GameObject.FindGameObjectWithTag("ChatManager");

        // Initialize lists
        nameList = new List<string>();
        colorList = new List<string>();

        newNames = new List<string>();
    }

    public void Start()
    {
        // Create instances of SheetWriter and SheetReader
        sheetWriter = new SheetWriter();
        sheetReader = new SheetReader();

        // Get sheet data and last row index
        sheetData = sheetReader.GetSheetData("Sheet1!A1:" + lastLetter + maxNum);
        lastRow = sheetReader.GetLastRow();

        Debug.Log("Last row: " + lastRow);

        IList<IList<object>> values = sheetData.values;

        // Read names and colors from the sheet data
        foreach (var row in values)
        {
            if (row.Count > 0)
            {
                // Read names from the first column (index 0)
                string name = row[0].ToString();
                nameList.Add(name);

                // Read colors from the second column (index 1)
                string color = row[1].ToString();
                colorList.Add(color);
            }
        }

        // Get new names that are not already in the nameList
        newNames = GetNewNames(nameList);
    }

    void Update()
    {
        if (Variables.Object(chatManager).Get<string>("newGuy1") != null)
        {
            string name = Variables.Object(chatManager).Get<string>("newGuy1");

            // Check the name of the new person
            CheckName(name);

            Variables.Object(chatManager).Set("newGuy1", null);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            // Write the new names to the sheet
            UpdateNames(newNames);
        }
    }

    void CheckName(string name)
    {
        // Whenever a new person joins the instance
        if (!nameList.Contains(name) && !newNames.Contains(name))
        {
            // Add them to the list of new names if they aren't already written down
            newNames.Add(name);
        }

        // Find the color for the person that joined
        int nameIndex = nameList.IndexOf(name);
        if (nameIndex != -1)
        {
            string color = colorList[nameIndex];
            Variables.Object(GameObject.Find(name)).Set("colorNum", color);
        }
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

    void UpdateNames(List<string> names)
    {
        var writeValues = new List<IList<object>>();

        int remainingSpace = maxNum - lastRow;

        int namesToWrite = Mathf.Min(names.Count, remainingSpace);

        // Iterate through the new names to be written
        for (int i = 0; i < namesToWrite; i++)
        {
            string name = names[i];

            // Check if the name is not already in the nameList
            if (!nameList.Contains(name))
            {
                var newRow = new List<object>();
                newRow.Add(name);
                writeValues.Add(newRow);
                nameList.Add(name); // Add the name to nameList
            }
        }

        // Write the new names to the sheet if there are any
        if (writeValues.Count > 0)
        {
            int startIndex = lastRow + 1;
            int endIndex = startIndex + writeValues.Count - 1;

            sheetWriter.WriteData("Sheet1", "A" + startIndex + ":" + lastLetter + endIndex, writeValues);

            lastRow = endIndex;
        }

        // Check if the maximum number of rows has been reached
        if (lastRow >= maxNum)
        {
            Debug.Log("MaxNum limit reached!");
        }

        newNames = GetNewNames(nameList); // Update newNames list after writing
    }
}