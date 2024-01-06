using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageSystemTest : MonoBehaviour
{
    public GameObject textBoxPrefab;
    public GameObject horizontalGroupPrefab; // Add a reference to your horizontal layout group prefab
    public Transform messageContainer;
    public float maxLineWidth = 400f;

    private Transform currentHorizontalGroup;
    private float currentLineWidth = 0f;

    public string testMessage = "";

    void Start()
    {
        CreateNewHorizontalGroup();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
                GameObject textBox = Instantiate(textBoxPrefab, currentHorizontalGroup);
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

                currentLineWidth += wordWidth;
            }

            // Start a new line for the next message
            currentLineWidth = 0f;
            MoveToNextLine();
        }
    }

    void MoveToNextLine()
    {
        // Move the current line to the next line
        foreach (Transform textBox in currentHorizontalGroup)
        {
            textBox.localPosition -= new Vector3(0f, LayoutUtility.GetPreferredHeight(textBox.GetComponent<RectTransform>()), 0f);
        }

        // Create a new horizontal layout group if needed
        if (currentLineWidth > maxLineWidth)
        {
            currentLineWidth = 0f;
            CreateNewHorizontalGroup();
        }
    }

    void CreateNewHorizontalGroup()
    {
        // Instantiate the horizontal layout group prefab
        GameObject newHorizontalGroup = Instantiate(horizontalGroupPrefab, messageContainer);
        currentHorizontalGroup = newHorizontalGroup.transform;
    }
}