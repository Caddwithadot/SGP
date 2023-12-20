/*******************************************************************************
Author: Taylor
State: Working
Description:
Handles the individual chatters seat and talk duration at the moment.
*******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chatter : MonoBehaviour
{
    private ChatterSpawner chatterSpawner;
    private ChatterManager chatterManager;
    private ChatManager chatManager;

    public TextMeshProUGUI chatterName;

    public int seatNum;

    public float talkDuration = 60f;
    public float timer;

    public int colorNum;

    private Animator jumpAnim;

    // Start is called before the first frame update
    void Start()
    {
        chatterSpawner = FindObjectOfType<ChatterSpawner>();
        chatterManager = FindObjectOfType<ChatterManager>();
        chatManager = FindObjectOfType<ChatManager>();

        //sets the display name as the chatters twitch name
        chatterName.text = name;

        jumpAnim = transform.GetChild(0).GetComponent<Animator>();

        timer = talkDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;

            //chatter no longer active
            if(timer <= 0)
            {
                //adds this chatters order number and position to the list
                chatterManager.seatList.Add(seatNum);
                chatterManager.positionList.Add(transform.position);

                //destroys chatter
                chatterSpawner.DestroyChatter(name);
            }
        }
    }

    //called from the ChatManager, meaning this chatter typed something
    public void ChatterTalked(string message)
    {
        timer = talkDuration;
        jumpAnim.SetTrigger("Jump");

        //chatter typed a colorCommand
        if(chatManager.colorCommands.Contains(message))
        {
            int index = chatManager.colorCommands.IndexOf(message);
            colorNum = index;
        }
    }
}
