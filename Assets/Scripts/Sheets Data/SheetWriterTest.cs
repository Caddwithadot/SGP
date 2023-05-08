using System.Collections.Generic;
using UnityEngine;

public class SheetWriterTest : MonoBehaviour
{
    private void Start()
    {
        var sheetWriter = new SheetWriter();
        var values = new List<IList<object>>
        {
            new List<object> { "Name", "Age" },
            new List<object> { "Alice", 25 },
            new List<object> { "Bob", 30 }
        };
        sheetWriter.WriteData("Sheet1", "A1:B3", values);
    }
}