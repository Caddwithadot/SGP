using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class Customization : MonoBehaviour
{
    public Material[] colors;
    public Renderer lilBod;
    private int color;

    private void Start()
    {
        color = Variables.Object(this).Get<int>("colorNum");
        SetColor(color);
    }

    private void Update()
    {
        if(color != Variables.Object(this).Get<int>("colorNum"))
        {
            SetColor(Variables.Object(this).Get<int>("colorNum"));
            color = Variables.Object(this).Get<int>("colorNum");
        }
    }

    public void SetColor(int colorIndex)
    {
        lilBod.material = colors[colorIndex];
    }
}
