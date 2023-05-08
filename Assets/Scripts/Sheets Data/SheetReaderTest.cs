using System.Collections.Generic;
using UnityEngine;

public class SheetReaderTest : MonoBehaviour
{
    void Start()
    {
        SheetReader sheetReader = new SheetReader();
        IList<IList<object>> values = sheetReader.getSheetRange("Sheet1!A1:C3");
        if (values != null)
        {
            foreach (var row in values)
            {
                foreach (var col in row)
                {
                    Debug.Log(col);
                }
            }
        }
    }
}