using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FontManager : MonoBehaviour
{
    public List<TMP_FontAsset> fontAssets;
    public GameObject textPrefab;
    public Transform parentObject;

    private Dictionary<TMP_FontAsset, Dictionary<char, float>> fontCharacterWidths = new Dictionary<TMP_FontAsset, Dictionary<char, float>>();

    public string testText;
    public TMP_FontAsset testFont;

    void Awake()
    {
        InstantiateCharacters();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GetSumOfWidths(testText, testFont);
        }
    }

    void InstantiateCharacters()
    {
        foreach (TMP_FontAsset fontAsset in fontAssets)
        {
            if (fontAsset == null)
            {
                Debug.LogWarning("Skipping null font asset in the list.");
                continue;
            }

            Dictionary<char, float> characterWidths = new Dictionary<char, float>();
            fontCharacterWidths.Add(fontAsset, characterWidths);

            string allCharacters = GetAllCharacters();

            foreach (char character in allCharacters)
            {
                GameObject textObject = Instantiate(textPrefab, parentObject);
                TextMeshProUGUI textComponent = textObject.GetComponent<TextMeshProUGUI>();

                // Set the font asset
                textComponent.font = fontAsset;

                // Set the character
                textComponent.text = character.ToString();

                // Print the preferred width
                float preferredWidth = LayoutUtility.GetPreferredWidth(textComponent.rectTransform);

                // Track the preferred width in the dictionary
                characterWidths.Add(character, preferredWidth);
            }
        }
    }

    string GetAllCharacters()
    {
        string allCharacters = "";
        for (char c = ' '; c <= '~'; ++c)
        {
            allCharacters += c;
        }
        return allCharacters;
    }

    // Method to get the sum of preferred widths for a given string and font asset name
    public float GetSumOfWidths(string text, TMP_FontAsset fontAsset)
    {
        float sumOfWidths = 0f;

        if (fontCharacterWidths.TryGetValue(fontAsset, out Dictionary<char, float> characterWidths))
        {
            foreach (char character in text)
            {
                if (characterWidths.TryGetValue(character, out float characterWidth))
                {
                    sumOfWidths += characterWidth;
                }
                else
                {
                    Debug.LogWarning($"Character '{character}' not found in the dictionary for font '{fontAsset.name}'. Skipping.");
                }
            }
        }
        else
        {
            Debug.LogWarning($"No dictionary found for font '{fontAsset.name}'.");
        }

        Debug.Log($"Sum of widths for font '{fontAsset.name}' and text '{text}': {sumOfWidths}");

        return sumOfWidths;
    }
}