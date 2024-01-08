using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FontManager : MonoBehaviour
{
    private HLG_Spacing hlgSpacing;

    public GameObject textPrefab;
    public Transform tempLayoutGroup;

    public List<TMP_FontAsset> fontAssets;
    private Dictionary<TMP_FontAsset, Dictionary<char, float>> fontCharacterWidths = new Dictionary<TMP_FontAsset, Dictionary<char, float>>();

    public TMP_FontAsset font;
    private TMP_FontAsset currentFont;

    public float fontSize = 36f;
    private float currentFontSize;
    private float defaultFontSize;

    private float textMultiplier;

    void Awake()
    {
        hlgSpacing = GetComponent<HLG_Spacing>();

        PopulateFontAssets();
        InstantiateCharacters();
    }

    private void Start()
    {
        currentFont = font;

        defaultFontSize = fontSize;
        currentFontSize = fontSize;

        textMultiplier = fontSize / defaultFontSize;
    }

    private void Update()
    {
        if(currentFontSize != fontSize || currentFont != font)
        {
            //multiplier used to calculate preferred widths
            textMultiplier = fontSize / defaultFontSize;

            //send the new font and/or font size
            hlgSpacing.FontChange(font, fontSize);

            //update current font and size
            currentFontSize = fontSize;
            currentFont = font;
        }
    }

    void PopulateFontAssets()
    {
        fontAssets.Clear();

        string[] guids = AssetDatabase.FindAssets("t:TMP_FontAsset");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);

            fontAssets.Add(fontAsset);
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
                if(character != ' ')
                {
                    GameObject textObject = Instantiate(textPrefab, tempLayoutGroup);
                    TextMeshProUGUI textComponent = textObject.GetComponent<TextMeshProUGUI>();

                    // Set the font size
                    textComponent.fontSize = fontSize;

                    // Set the font asset
                    textComponent.font = fontAsset;

                    // Set the character
                    textComponent.text = character.ToString();

                    // Print the preferred width
                    float preferredWidth = LayoutUtility.GetPreferredWidth(textComponent.rectTransform);

                    // Track the preferred width in the dictionary
                    characterWidths.Add(character, preferredWidth);
                }
                else
                {
                    // Get spacing value from HLG_Spacing for the current font asset
                    float spacing = hlgSpacing.GetSpacingForFont(fontAsset);

                    // Track the preferred width in the dictionary
                    characterWidths.Add(character, spacing);
                }
            }
        }

        //destroy the temporary layout group after getting all font character widths
        //Destroy(tempLayoutGroup.gameObject);

        // Print debug messages for character widths
        foreach (TMP_FontAsset fontAsset in fontAssets)
        {
            if (fontAsset.name.Contains("Liberation"))
            {
                PrintCharacterWidths(fontAsset);
            }
        }
    }

    public void PrintCharacterWidths(TMP_FontAsset fontAsset)
    {
        if (fontCharacterWidths.TryGetValue(fontAsset, out Dictionary<char, float> characterWidths))
        {
            Debug.Log($"Character Widths for Font '{fontAsset.name}':");

            foreach (var kvp in characterWidths)
            {
                char character = kvp.Key;
                float width = kvp.Value;

                Debug.Log($"Character: '{character}', Width: {width}");
            }
        }
        else
        {
            Debug.LogWarning($"No dictionary found for font '{fontAsset.name}'.");
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
                    Debug.Log(textMultiplier);
                    // Sum the adjusted preferred widths
                    sumOfWidths += (characterWidth * textMultiplier);
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