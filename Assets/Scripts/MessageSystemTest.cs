using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageSystemTest : MonoBehaviour
{
    public GameObject textBoxPrefab;
    public Transform horizontalGroupParent;

    public string testMessage = "";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
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
                GameObject textBox = Instantiate(textBoxPrefab, horizontalGroupParent);
                TextMeshProUGUI textComponent = textBox.GetComponentInChildren<TextMeshProUGUI>();
                textComponent.text = word;
            }
        }
    }
}