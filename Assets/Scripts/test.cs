using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class test : MonoBehaviour
{
    public FontManager fontManager;

    public TMP_FontAsset testFont;
    public string testText;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            fontManager.GetSumOfWidths(testText, testFont);
        }
    }
}
