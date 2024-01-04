using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageSystemTest : MonoBehaviour
{
    public GameObject textBoxPrefab;
    public Transform messageContainer;
    public float maxLineWidth = 400f;

    private List<Transform> currentLine = new List<Transform>();
    private float currentLineWidth = 0f;

    public string testMessage = "";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SendMessage(testMessage);
        }
    }

    void SendMessage(params string[] messages)
    {
        foreach (string message in messages)
        {
            string[] words = message.Split(' ');

            foreach (string word in words)
            {
                GameObject textBox = Instantiate(textBoxPrefab, messageContainer);
                TextMeshProUGUI textComponent = textBox.GetComponentInChildren<TextMeshProUGUI>();
                textComponent.text = word;

                float wordWidth = LayoutUtility.GetPreferredWidth(textComponent.rectTransform);

                // Check if adding the word exceeds the max line width
                if (currentLineWidth + wordWidth > maxLineWidth)
                {
                    // Start a new line
                    currentLineWidth = 0f;
                    MoveToNextLine();
                }

                currentLine.Add(textBox.transform);
                currentLineWidth += wordWidth;
            }

            // Start a new line for the next message
            currentLineWidth = 0f;
            MoveToNextLine();
        }
    }

    void MoveToNextLine()
    {
        if (currentLine.Count > 0)
        {
            // Move the current line to the next line
            foreach (Transform textBox in currentLine)
            {
                textBox.localPosition -= new Vector3(0f, LayoutUtility.GetPreferredHeight(textBox.GetComponent<RectTransform>()), 0f);
            }

            // Clear the current line
            currentLine.Clear();
        }
    }
}