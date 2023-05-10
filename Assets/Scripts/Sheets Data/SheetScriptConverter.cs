using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SheetScriptConverter : MonoBehaviour
{
    public void Start()
    {
        string wah = Variables.Object(gameObject).Get<string>("test");

        SheetReader sheetReader = new SheetReader();
        SheetWriter sheetWriter = new SheetWriter();

        var writeValues = new List<IList<object>>
        {
            new List<object> { "A1", "B1" },
            new List<object> { "A2", "B2" },
            new List<object> { "A3", "B3", "C3" }
        };
        sheetWriter.WriteData("Sheet1", "A1:C3", writeValues);

        IList<IList<object>> readValues = sheetReader.getSheetRange("Sheet1!A1:C3");
        if (readValues != null)
        {
            foreach (var row in readValues)
            {
                foreach (var col in row)
                {
                    Debug.Log(col);
                }
            }
        }
    }
}