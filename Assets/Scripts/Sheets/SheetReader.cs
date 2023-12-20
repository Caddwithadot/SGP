using System;
using System.Collections.Generic;
using System.IO;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using UnityEngine;
using System.Collections;
using System.Linq;

public class SheetReader
{
    static private String spreadsheetId = "1Z9mVBPT_RIk2siQ3Xr00rH4d39ng64pQMsNRFpGI2Ow";
    static private String jsonPath = "/StreamingAssets/Credentials/streamergp-0632d6337c35.json";

    static private SheetsService service;

    static private int lastRow;

    static SheetReader()
    {
        String fullJsonPath = Application.dataPath + jsonPath;

        Stream jsonCreds = (Stream)File.Open(fullJsonPath, FileMode.Open);

        ServiceAccountCredential credential = ServiceAccountCredential.FromServiceAccountData(jsonCreds);

        service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
        });
    }

    public SheetData GetSheetData(string sheetNameAndRange)
    {
        SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, sheetNameAndRange);

        ValueRange response = request.Execute();
        IList<IList<object>> values = response.Values;

        if (values != null && values.Count > 0)
        {
            int numRows = values.Count;
            int numColumns = values[0].Count;

            lastRow = numRows;

            return new SheetData(numRows, numColumns, values);
        }
        else
        {
            Debug.Log("No data found.");
            return null;
        }
    }

    public int GetLastRow()
    {
        return lastRow;
    }
}

public class SheetData
{
    public int numRows;
    public int numColumns;
    public IList<IList<object>> values;

    public SheetData(int numRows, int numColumns, IList<IList<object>> values)
    {
        this.numRows = numRows;
        this.numColumns = numColumns;
        this.values = values;
    }
}