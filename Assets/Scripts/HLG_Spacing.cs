using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FontSpacingEntry
{
    public TMP_FontAsset fontAsset;
    public float spacing;
}

public class HLG_Spacing : MonoBehaviour
{
    private FontManager fontManager;

    public List<FontSpacingEntry> HLGSpacingEntries;
    public Dictionary<TMP_FontAsset, float> HLGSpacingMap = new Dictionary<TMP_FontAsset, float>();

    private float defaultFontSize;
    private float defaultSpacing;
    private float scaledSpacing;

    public Transform chatVerticalLayout;

    private void Start()
    {
        fontManager = GetComponent<FontManager>();
        defaultFontSize = fontManager.fontSize;
    }

    public void FontChange(TMP_FontAsset newFont, float newSize)
    {
        SetFontSpacing(newFont, newSize);

        // Set the default spacing based on the current font asset
        if (HLGSpacingMap.TryGetValue(newFont, out float currentFontSpacing))
        {
            defaultSpacing = currentFontSpacing;
            scaledSpacing = defaultSpacing * fontManager.textMultiplier;

            // Apply the scaled spacing to the HorizontalLayoutGroup
            UpdateChildLayoutGroupsSpacing(scaledSpacing);

            // Update the scale of all child TextMeshPro components based on the fontSize
            UpdateChildTextScales(newSize);
        }
        else
        {
            Debug.LogWarning($"Spacing not found for current font asset '{newFont.name}'.");
        }
    }

    void SetFontSpacing(TMP_FontAsset newFont, float newSize)
    {
        foreach (FontSpacingEntry entry in HLGSpacingEntries)
        {
            if (entry.fontAsset == null)
            {
                Debug.LogWarning("Skipping null font asset in the list.");
                continue;
            }

            // Save the spacing value in the dictionary
            HLGSpacingMap[entry.fontAsset] = entry.spacing;
        }

        // Apply spacing for the current font asset
        UpdateFontAndSpacing(newFont, newSize);
    }

    void UpdateFontAndSpacing(TMP_FontAsset newFont, float newSize)
    {
        if (HLGSpacingMap.TryGetValue(newFont, out float spacing))
        {
            TextMeshProUGUI[] textComponents = chatVerticalLayout.GetComponentsInChildren<TextMeshProUGUI>(true);

            foreach (TextMeshProUGUI textComponent in textComponents)
            {
                if (textComponent != null)
                {
                    textComponent.font = newFont;
                }
            }

            // Update defaultSpacing and scaledSpacing based on the new font asset
            float newDefaultSpacing = spacing;
            //float newSizeDifference = newSize / defaultFontSize;
            defaultSpacing = newDefaultSpacing * fontManager.textMultiplier;
            scaledSpacing = newDefaultSpacing;

            // Apply the scaled spacing to the HorizontalLayoutGroup
            UpdateChildLayoutGroupsSpacing(scaledSpacing);

            // Update the scale of all child TextMeshPro components based on the fontSize
            UpdateChildTextScales(newSize);
        }
        else
        {
            Debug.LogWarning($"Spacing not found for font '{newFont.name}'.");
        }
    }

    void UpdateChildTextScales(float newSize)
    {
        TextMeshProUGUI[] textComponents = chatVerticalLayout.GetComponentsInChildren<TextMeshProUGUI>(true);

        foreach (TextMeshProUGUI textComponent in textComponents)
        {
            // Update the scale of the text component based on the fontSize
            textComponent.fontSize = newSize;
        }
    }

    void UpdateChildLayoutGroupsSpacing(float spacing)
    {
        foreach (Transform child in chatVerticalLayout)
        {
            HorizontalLayoutGroup layoutGroup = child.GetComponent<HorizontalLayoutGroup>();
            if (layoutGroup != null)
            {
                // Update the spacing of the HorizontalLayoutGroup component
                layoutGroup.spacing = spacing;
            }
        }
    }

    public float GetSpacingForFont(TMP_FontAsset fontAsset)
    {
        foreach (FontSpacingEntry entry in HLGSpacingEntries)
        {
            if (entry.fontAsset == fontAsset)
            {
                return entry.spacing;
            }
        }

        Debug.LogWarning($"Spacing not found for font asset '{fontAsset.name}'. Returning default spacing.");
        return 0f; // Return a default value or handle it based on your requirements
    }

}