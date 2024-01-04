using TMPro;
using UnityEngine;

public class MeasureTextTest : MonoBehaviour
{
    public TMP_FontAsset font; // Set your font asset in the Unity Editor

    void Start()
    {
        MeasureText("Test");
    }

    public void MeasureText(string message)
    {
        string textToMeasure = message;

        // Create a temporary TextMeshPro object
        GameObject tempObject = new GameObject("TempText");
        TextMeshProUGUI tempText = tempObject.AddComponent<TextMeshProUGUI>();

        // Set the font
        tempText.font = font;

        // Set the text content
        tempText.text = textToMeasure;

        // Get the preferred values
        Vector2 preferredValues = tempText.GetPreferredValues(textToMeasure);

        // Print the preferred width
        float preferredWidth = preferredValues.x;
        Debug.Log("Preferred Width: " + preferredWidth);

        Destroy(tempObject);
    }
}