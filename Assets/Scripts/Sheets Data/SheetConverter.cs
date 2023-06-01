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
    private List<string> newColors;

    private void Awake()
    {
        // Find the ChatManager GameObject
        chatManager = GameObject.FindGameObjectWithTag("ChatManager");

        // Initialize lists
        nameList = new List<string>();
        colorList = new List<string>();

        newNames = new List<string>();
        newColors = new List<string>();
    }

    public void Start()
    {
        // Create instances of SheetWriter and SheetReader
        sheetWriter = new SheetWriter();
        sheetReader = new SheetReader();

        // Get sheet data and last row index
        sheetData = sheetReader.GetSheetData("Sheet1!A2:" + lastLetter + maxNum);
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
        // New chatter spawned
        if (Variables.Object(chatManager).Get<string>("newGuy1") != null)
        {
            string name = Variables.Object(chatManager).Get<string>("newGuy1");

            // Check the name of the new person
            CheckName(name);

            Variables.Object(chatManager).Set("newGuy1", null);
        }

        // Chatter has changed something
        if (Variables.Object(this.gameObject).Get<string>("name") != null)
        {
            string name = Variables.Object(this.gameObject).Get<string>("name");

            // The chatter's color has been changed
            if (Variables.Object(this.gameObject).Get<string>("colorChange") != null)
            {
                string color = Variables.Object(this.gameObject).Get<string>("colorChange");
                UpdateColor(name, color);

                Variables.Object(this.gameObject).Set("colorChange", null);
            }

            Variables.Object(this.gameObject).Set("name", null);
        }

        // TEMPORARY
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Update the colors
            List<string> allColors = new List<string>(colorList);
            allColors.AddRange(newColors);

            // Write the new shit on the sheet
            WriteShit(newNames, allColors);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach(string color in colorList)
            {
                Debug.Log(color);
            }
        }
    }

    void UpdateColor(string name, string color)
    {
        int index = nameList.IndexOf(name);

        // If the name is already written down and not a new name
        if (index != -1)
        {
            colorList[index] = color;
        }
        else if (newNames.Contains(name))
        {
            index = newNames.IndexOf(name);
            newColors[index] = color;
        }
    }

    void CheckName(string name)
    {
        // When a new person joins and is not written down
        if (!nameList.Contains(name) && !newNames.Contains(name))
        {
            // Add them to the list of new names if they aren't already written down
            newNames.Add(name);

            // Add default color to the new color list for when a new person joins
            newColors.Add("0");
        }

        // Find the color for the written person that joined
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

    void WriteShit(List<string> names, List<string> colors)
    {
        var writeNameValues = new List<IList<object>>();
        var writeColorValues = new List<IList<object>>();

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
                writeNameValues.Add(newRow);
                nameList.Add(name); // Add the name to nameList
            }
        }

        // Create color values for each name
        foreach (string color in colors)
        {
            var newColorRow = new List<object>();
            newColorRow.Add(color);
            writeColorValues.Add(newColorRow);
        }

        int startIndex = lastRow + 2;
        int endIndex = startIndex + writeNameValues.Count - 1;

        // Write the new names to the sheet if there are any
        if (writeNameValues.Count > 0)
        {
            sheetWriter.WriteData("Sheet1", "A" + startIndex + ":A" + endIndex, writeNameValues);

            lastRow = endIndex;
        }

        // Update all the colors
        sheetWriter.WriteData("Sheet1", "B2:B" + endIndex, writeColorValues);

        // Check if the maximum number of rows has been reached
        if (lastRow >= maxNum)
        {
            Debug.Log("MaxNum limit reached!");
        }

        newColors.Clear();
        newNames.Clear();
    }
}