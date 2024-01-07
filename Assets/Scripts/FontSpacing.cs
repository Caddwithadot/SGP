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

public class FontSpacing : MonoBehaviour
{
    public List<FontSpacingEntry> fontSpacingEntries;
    public HorizontalLayoutGroup horizontalLayoutGroup;
    public TMP_FontAsset currentFontAsset;

    private TMP_FontAsset newFontAsset;

    private Dictionary<TMP_FontAsset, float> fontSpacingMap = new Dictionary<TMP_FontAsset, float>();

    private float defaultSpacing;
    private float scaledSpacing;

    private float defaultFontSize = 36f;
    private float currentFontSize;
    public float fontSize;

    private void Start()
    {
        fontSize = defaultFontSize;
        currentFontSize = fontSize;
        newFontAsset = currentFontAsset;

        SetFontSpacing();
    }

    // Update is called once per frame
    void Update()
    {
        if (newFontAsset != currentFontAsset || currentFontSize != fontSize)
        {
            SetFontSpacing();

            // Set the default spacing based on the current font asset
            if (fontSpacingMap.TryGetValue(currentFontAsset, out float currentFontSpacing))
            {
                defaultSpacing = currentFontSpacing;
                scaledSpacing = defaultSpacing * (fontSize / defaultFontSize);

                // Apply the scaled spacing to the HorizontalLayoutGroup
                horizontalLayoutGroup.spacing = scaledSpacing;

                // Update the scale of all child TextMeshPro components based on the fontSize
                UpdateChildTextScales();
            }
            else
            {
                Debug.LogWarning($"Spacing not found for current font asset '{currentFontAsset.name}'.");
            }

            

            currentFontSize = fontSize;
            newFontAsset = currentFontAsset;
        }
    }

    void SetFontSpacing()
    {
        foreach (FontSpacingEntry entry in fontSpacingEntries)
        {
            if (entry.fontAsset == null)
            {
                Debug.LogWarning("Skipping null font asset in the list.");
                continue;
            }

            // Save the spacing value in the dictionary
            fontSpacingMap[entry.fontAsset] = entry.spacing;
        }

        // Apply spacing for the current font asset
        UpdateFontAndSpacing(currentFontAsset);
    }

    public void UpdateFontAndSpacing(TMP_FontAsset fontAsset)
    {
        if (fontSpacingMap.TryGetValue(fontAsset, out float spacing))
        {
            // Update the font of all child TextMeshPro components
            foreach (Transform child in transform)
            {
                TextMeshProUGUI textComponent = child.GetComponent<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.font = fontAsset;
                }
            }

            // Update defaultSpacing and scaledSpacing based on the new font asset
            float newDefaultSpacing = spacing;
            float newSizeDifference = fontSize / defaultFontSize;
            defaultSpacing = newDefaultSpacing * newSizeDifference;
            scaledSpacing = newDefaultSpacing;

            // Apply the scaled spacing to the HorizontalLayoutGroup
            horizontalLayoutGroup.spacing = scaledSpacing;

            // Update the scale of all child TextMeshPro components based on the fontSize
            UpdateChildTextScales();
        }
        else
        {
            Debug.LogWarning($"Spacing not found for font '{fontAsset.name}'.");
        }
    }

    void UpdateChildTextScales()
    {
        foreach (Transform child in transform)
        {
            TextMeshProUGUI textComponent = child.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                // Update the scale of the text component based on the fontSize
                textComponent.fontSize = fontSize;
            }
        }
    }
}