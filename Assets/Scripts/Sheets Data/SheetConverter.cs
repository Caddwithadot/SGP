using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SheetConverter : MonoBehaviour
{
    [SerializeField]
    private string lastLetter;

    [SerializeField]
    private int maxNum = 1000;

    public void Start()
    {
        //string wah = Variables.Object(gameObject).Get<string>("test");

        SheetWriter sheetWriter = new SheetWriter();
        SheetReader sheetReader = new SheetReader();

        SheetData sheetData = sheetReader.GetSheetData("Sheet1!A1:" + lastLetter + maxNum);

        Debug.Log("Last row: " + sheetReader.GetLastRow());

        //READER//

        if (sheetData != null)
        {
            IList<IList<object>> values = sheetData.values;

            if (values != null)
            {
                foreach (var row in values)
                {
                    foreach (var col in row)
                    {
                        //Debug.Log(col);
                    }
                }

                Variables.Object(gameObject).Set("Columns", sheetData.numColumns);
                Variables.Object(gameObject).Set("Rows", sheetData.numRows);
            }
        }

        //WRITER//

        var writeValues = new List<IList<object>>
        {
            new List<object> { "A1", "B1" },
            new List<object> { "A2", "B2" },
            new List<object> { "A3", "B3", "C3", "D3" }
        };
        sheetWriter.WriteData("Sheet1", "A1:" + lastLetter + "3", writeValues);
    }
}