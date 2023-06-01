using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FinalConverter : MonoBehaviour
{
    public int maxNum;

    private SheetWriter sheetWriter;
    private SheetReader sheetReader;
    private SheetData sheetData;
    private int lastRow;

    private GameObject chatManager;
    private List<string> nameList;
    private List<string> colorList;

    private void Awake()
    {
        // Find the ChatManager GameObject
        chatManager = GameObject.FindGameObjectWithTag("ChatManager");

        // Initialize lists
        nameList = new List<string>();
        colorList = new List<string>();
    }

    public void Start()
    {
        // Create instances of SheetWriter and SheetReader
        sheetWriter = new SheetWriter();
        sheetReader = new SheetReader();

        // Get sheet data and last row index
        sheetData = sheetReader.GetSheetData("Sheet1!A2:B" + maxNum);
        lastRow = sheetReader.GetLastRow();

        IList<IList<object>> values = sheetData.values;

        // Read names and colors from the sheet data
        foreach (var row in values)
        {
            if (row.Count > 0)
            {
                // Read names
                string name = row[0].ToString();
                nameList.Add(name);

                // Read colors
                string color = row[1].ToString();
                colorList.Add(color);
            }
        }
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

        if (Input.GetKeyDown(KeyCode.F))
        {
            WriteShit(nameList, colorList);
        }
    }

    void CheckName(string name)
    {
        // When a new person joins and is not written down
        if (!nameList.Contains(name))
        {
            // Add them to the list if the name is not written down
            nameList.Add(name);
            colorList.Add(Variables.Object(GameObject.Find(name)).Get<string>("colorNum"));
        }
        else
        {
            // Find the color for the written person that joined
            string color = colorList[nameList.IndexOf(name)];
            Variables.Object(GameObject.Find(name)).Set("colorNum", color);
            Variables.Object(GameObject.Find(name)).Set("start", true);
        }
    }

    void UpdateColor(string name, string color)
    {
        int index = nameList.IndexOf(name);

        colorList[index] = color;
    }

    void WriteShit(List<string> names, List<string> colors)
    {
        var writeNameValues = new List<IList<object>>();
        var writeColorValues = new List<IList<object>>();

        // Do names
        foreach (string name in names)
        {
            var nameRow = new List<object>();
            nameRow.Add(name);
            writeNameValues.Add(nameRow);
        }

        // Do colors
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