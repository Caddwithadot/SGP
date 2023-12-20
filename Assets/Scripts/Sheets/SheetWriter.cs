using System;
using System.Collections.Generic;
using System.IO;

using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

using UnityEngine;

public class SheetWriter
{
    private static readonly string SpreadsheetId = "1Z9mVBPT_RIk2siQ3Xr00rH4d39ng64pQMsNRFpGI2Ow";
    private static readonly string JsonPath = "/StreamingAssets/Credentials/streamergp-0632d6337c35.json";

    private static SheetsService _service;

    static SheetWriter()
    {
        string fullJsonPath = Application.dataPath + JsonPath;
        using (Stream jsonCreds = new FileStream(fullJsonPath, FileMode.Open, FileAccess.Read))
        {
            var credential = GoogleCredential.FromStream(jsonCreds)
                .CreateScoped(new[] { SheetsService.Scope.Spreadsheets });

            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });
        }
    }

    public void WriteData(string sheetName, string range, IList<IList<object>> values)
    {
        var valueRange = new ValueRange { Values = values };
        var updateRequest = _service.Spreadsheets.Values.Update(valueRange, SpreadsheetId, range);
        updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
        updateRequest.Execute();
    }
}