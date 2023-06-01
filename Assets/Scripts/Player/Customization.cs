using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using Photon.Chat;
using System;
using System.Linq;

public class Customization : MonoBehaviour
{
    public Material[] colors;
    public Renderer lilBod;
    private string color;

    private GameObject dataManager;

    private void Awake()
    {
        dataManager = GameObject.Find("_DataManager");
    }

    private void Start()
    {
        color = Variables.Object(this).Get<string>("colorNum");
        int index = int.Parse(color);
        lilBod.material = colors[index];
    }

    private void Update()
    {
        if(Variables.Object(this).Get<string>("colorNum") != "")
        {
            if (color != Variables.Object(this).Get<string>("colorNum"))
            {
                SetColor(Variables.Object(this).Get<string>("colorNum"));
                color = Variables.Object(this).Get<string>("colorNum");
            }
        }
    }

    public void SetColor(string colorIndex)
    {
        int index = int.Parse(colorIndex);
        lilBod.material = colors[index];

        Variables.Object(dataManager).Set("name", this.gameObject.name);
        Variables.Object(dataManager).Set("colorChange", Variables.Object(this).Get<string>("colorNum"));
    }
}
