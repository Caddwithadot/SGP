/*******************************************************************************
Author: Taylor
State: Mostly complete, need to add emotes still and figure out what to do to fix pre-exisiting messages in the in-game chat.
Description:
Stores all fonts and basically acts as a custom messaging system.
*******************************************************************************/

using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FontManager : MonoBehaviour
{
    private HLG_Spacing hlgSpacing;

    public RectTransform chatCanvas;
    public Transform verticalParent;

    public GameObject horizontalParentPrefab;
    public GameObject textPrefab;

    public Transform tempLayoutGroup;

    public List<TMP_FontAsset> fontAssets;
    private Dictionary<TMP_FontAsset, Dictionary<char, float>> fontCharacterWidths = new Dictionary<TMP_FontAsset, Dictionary<char, float>>();

    public TMP_FontAsset font;
    private TMP_FontAsset currentFont;

    public float fontSize = 36f;
    private float currentFontSize;
    private float defaultFontSize;

    [HideInInspector]
    public float textMultiplier;
    private float lineThreshold = 400f;

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
        //updates the threshold for message wrapping
        lineThreshold = chatCanvas.sizeDelta.x;

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
        Destroy(tempLayoutGroup.gameObject);
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

    public void InGameMessage(string text)
    {
        float sumOfWidths = 0f;
        float currentLineWidth = 0f;
        string currentLine = "";

        if (fontCharacterWidths.TryGetValue(font, out Dictionary<char, float> characterWidths))
        {
            foreach (char character in text)
            {
                if (characterWidths.TryGetValue(character, out float characterWidth))
                {
                    float adjustedWidth = characterWidth * textMultiplier;

                    // Check if adding the character exceeds the line threshold
                    if (currentLineWidth + adjustedWidth > lineThreshold)
                    {
                        // Find the latest space within the line and split the string
                        int lastSpaceIndex = currentLine.LastIndexOf(' ');

                        if (lastSpaceIndex != -1)
                        {
                            string lineToPrint = currentLine.Substring(0, lastSpaceIndex);

                            //send the line to be created
                            CreateLine(lineToPrint);

                            //reset current line
                            currentLine = currentLine.Substring(lastSpaceIndex + 1);
                            currentLineWidth = 0f;
                        }
                        else
                        {
                            // If there is no space, print the line and reset variables for the new line
                            //send the line to be created
                            CreateLine(currentLine);

                            //reset current line
                            currentLine = "";
                            currentLineWidth = 0f;
                        }
                    }

                    // Sum the adjusted preferred widths
                    sumOfWidths += adjustedWidth;

                    // Add the character to the current line
                    currentLine += character;
                    currentLineWidth += adjustedWidth;
                }
                else
                {
                    Debug.LogWarning($"Character '{character}' not found in the dictionary for font '{font.name}'. Skipping.");
                }
            }

            // Print the last line if it's not empty
            if (!string.IsNullOrEmpty(currentLine))
            {
                //send the final line to be created
                CreateLine(currentLine);
            }
        }
    }

    void CreateLine(params string[] messageLine)
    {
        foreach (string message in messageLine)
        {
            string[] words = message.Split(' ');

            GameObject horizontalParent = Instantiate(horizontalParentPrefab, verticalParent);

            if (fontCharacterWidths.TryGetValue(font, out Dictionary<char, float> characterWidths) && characterWidths.TryGetValue(' ', out float spaceWidth))
            {
                //update the spacing of the new hlg
                horizontalParent.GetComponent<HorizontalLayoutGroup>().spacing = spaceWidth * textMultiplier;
            }

            //instantiate text boxes
            foreach (string word in words)
            {
                GameObject textBox = Instantiate(textPrefab, horizontalParent.transform);
                TextMeshProUGUI textComponent = textBox.GetComponentInChildren<TextMeshProUGUI>();

                textComponent.font = font;
                textComponent.fontSize = fontSize;
                textComponent.text = word;
            }
        }
    }
}