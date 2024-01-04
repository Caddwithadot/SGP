using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLayoutTest : MonoBehaviour
{
    public GameObject prefab;
    public Transform container;
    public float maxLineWidth = 400f;

    private List<Transform> instantiatedElements = new List<Transform>();
    private float currentLineWidth = 0f;

    void Start()
    {
        InstantiateElement();
    }

    void InstantiateElement()
    {
        GameObject newElement = Instantiate(prefab, container);
        instantiatedElements.Add(newElement.transform);

        // Check if the new element exceeds the maximum line width
        if (currentLineWidth + LayoutUtility.GetPreferredWidth(newElement.GetComponent<RectTransform>()) > maxLineWidth)
        {
            // Move to the next line
            currentLineWidth = 0f;
            MoveToNextLine();
        }

        // Update the current line width
        currentLineWidth += LayoutUtility.GetPreferredWidth(newElement.GetComponent<RectTransform>());
    }

    void MoveToNextLine()
    {
        foreach (Transform element in instantiatedElements)
        {
            element.localPosition -= new Vector3(0f, LayoutUtility.GetPreferredHeight(element.GetComponent<RectTransform>()), 0f);
        }
    }
}