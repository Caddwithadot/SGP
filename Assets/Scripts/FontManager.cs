using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FontManager : MonoBehaviour
{
    public List<TMP_FontAsset> fontAssets;
    public GameObject textPrefab;
    public Transform tempLayoutGroup;

    private Dictionary<TMP_FontAsset, Dictionary<char, float>> fontCharacterWidths = new Dictionary<TMP_FontAsset, Dictionary<char, float>>();

    public float defaultFontSize = 36f;

    void Awake()
    {
        PopulateFontAssets();
        InstantiateCharacters();
    }

    void PopulateFontAssets()
    {
        fontAssets.Clear();

        string[] guids = AssetDatabase.FindAssets("t:TMP_FontAsset");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);

            if (fontAsset != null)
            {
                fontAssets.Add(fontAsset);
            }
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
                GameObject textObject = Instantiate(textPrefab, tempLayoutGroup);
                TextMeshProUGUI textComponent = textObject.GetComponent<TextMeshProUGUI>();
                
                // Set the font size
                textComponent.fontSize = defaultFontSize;

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

        //destroy the temporary layout group after getting all font character widths
        Destroy(tempLayoutGroup.gameObject);

        // Print information for each font asset in the dictionary
        foreach (var kvp in fontCharacterWidths)
        {
            TMP_FontAsset currentFontAsset = kvp.Key;
            Dictionary<char, float> currentCharacterWidths = kvp.Value;

            Debug.Log($"Font Asset: {currentFontAsset.name}");

            foreach (var characterWidth in currentCharacterWidths)
            {
                char character = characterWidth.Key;
                float width = characterWidth.Value;

                Debug.Log($"Character: '{character}', Width: {width}");
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

    // need to update this
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