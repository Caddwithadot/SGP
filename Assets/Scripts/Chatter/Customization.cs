/*******************************************************************************
Author: Taylor
State: Working
Description:
Handles all customization for chatters.
*******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customization : MonoBehaviour
{
    private FinalConverter finalConverter;
    private Chatter chatterComponent;

    public Material[] colors;
    public Renderer lilBod;
    private int color;

    private void Awake()
    {
        finalConverter = FindObjectOfType<FinalConverter>();
    }

    private void Start()
    {
        chatterComponent = GetComponent<Chatter>();
        color = chatterComponent.colorNum;

        //set original chatter color
        int index = color;
        lilBod.material = colors[index];
    }

    private void Update()
    {
        //checks if the current color is the same as the Chatter.cs
        if (color != chatterComponent.colorNum)
        {
            SetColor(chatterComponent.colorNum);
        }
    }

    //updates the color if not the same as Chatter.cs
    public void SetColor(int colorIndex)
    {
        //sets the new color of the chatter
        color = colorIndex;
        lilBod.material = colors[colorIndex];

        //sends updated color to FinalConverter.cs
        finalConverter.UpdateColor(name, color.ToString());
    }
}
