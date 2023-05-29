using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class Customization : MonoBehaviour
{
    public Material[] colors;
    public Renderer lilBod;
    private string color;

    private void Start()
    {
        color = Variables.Object(this).Get<string>("colorNum");
        //int randomIndex = Random.Range(0, colors.Length);
        //SetColor(randomIndex.ToString());
        SetColor(color);
    }

    private void Update()
    {
        if(color != Variables.Object(this).Get<string>("colorNum"))
        {
            SetColor(Variables.Object(this).Get<string>("colorNum"));
            color = Variables.Object(this).Get<string>("colorNum");
        }
    }

    public void SetColor(string colorIndex)
    {
        int index = int.Parse(colorIndex);
        lilBod.material = colors[index];
    }
}
