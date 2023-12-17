/*******************************************************************************
Author: Taylor
State: Needs a second look after the dataManager gets swapped to c#
Description:
Currently handles color customization for the chatter.
*******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class Customization : MonoBehaviour
{
    private Chatter chatter;

    public Material[] colors;
    public Renderer lilBod;
    private int color;

    private GameObject dataManager;

    private void Awake()
    {
        dataManager = GameObject.Find("_DataManager");
    }

    private void Start()
    {
        chatter = GetComponent<Chatter>();
        color = chatter.colorNum;

        //set original chatter color
        int index = color;
        lilBod.material = colors[index];
    }

    private void Update()
    {
        if (color != chatter.colorNum)
        {
            SetColor(chatter.colorNum);
        }
    }

    public void SetColor(int colorIndex)
    {
        //int index = int.Parse(colorIndex);
        color = colorIndex;
        lilBod.material = colors[colorIndex];

        Variables.Object(dataManager).Set("name", this.gameObject.name);
        Variables.Object(dataManager).Set("colorChange", Variables.Object(this).Get<string>("colorNum"));
    }
}
