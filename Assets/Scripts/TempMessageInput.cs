using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMessageInput : MonoBehaviour
{
    private FontManager fontManager;

    public string message;

    // Start is called before the first frame update
    void Start()
    {
        fontManager = GetComponent<FontManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            fontManager.InGameMessage(message);
        }
    }
}
