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

    private bool isWritingNewRow = false;

    public void Start()
    {
        sheetWriter = new SheetWriter();
        sheetReader = new SheetReader();
        sheetData = sheetReader.GetSheetData("Sheet1!A1:" + lastLetter + maxNum);
        lastRow = sheetReader.GetLastRow();

        Debug.Log("Last row: " + lastRow);

        // READER //

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
    }

    void Update()
    {
        // WRITER //
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            lastRow += 1;
            Debug.Log("Last row: " + lastRow);

            isWritingNewRow = true;
        }

        if (isWritingNewRow)
        {
            var writeValues = new List<IList<object>>();
            var newRow = new List<object>();

            for (int col = 0; col < sheetData.numColumns; col++)
            {
                newRow.Add(col + 1);
            }

            writeValues.Add(newRow);

            sheetWriter.WriteData("Sheet1", "A" + lastRow + ":" + lastLetter + lastRow, writeValues);

            isWritingNewRow = false;
        }
        */
    }
}