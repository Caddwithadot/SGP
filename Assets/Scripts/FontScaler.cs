using UnityEngine;
using TMPro;

public class FontScaler : MonoBehaviour
{
    public float minFontSize = 10f;
    public float maxFontSize = 100f;
    public int characterThreshold = 50;

    private TextMeshProUGUI textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        int textLength = textComponent.text.Length;
        float scaleFactor;

        if (textLength <= characterThreshold)
        {
            scaleFactor = Mathf.Lerp(maxFontSize, minFontSize, Mathf.InverseLerp(0, characterThreshold, textLength));
        }
        else
        {
            scaleFactor = minFontSize - (textLength - characterThreshold);
        }

        textComponent.fontSize = Mathf.Max(scaleFactor, minFontSize);
    }
}