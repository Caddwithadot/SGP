/*******************************************************************************
Author: Taylor
State: Needs a second look after the playerList application variable is changed.
Description:
Handles the individual chatters seat and talk duration at the moment.
*******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chatter : MonoBehaviour
{
    public TextMeshProUGUI chatterName;
    private ChatterManager chatterManager;
    private ChatManager chatManager;

    public int seatNum;

    public float talkDuration = 60f;
    private float timer;

    public int colorNum;

    private Animator jumpAnim;

    // Start is called before the first frame update
    void Start()
    {
        chatterManager = FindObjectOfType<ChatterManager>();
        chatManager = FindObjectOfType<ChatManager>();

        //sets the display name as the chatters twitch name
        chatterName.text = this.name;

        jumpAnim = transform.GetChild(0).GetComponent<Animator>();

        timer = talkDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                //adds this chatters order number and position to the list
                chatterManager.seatList.Add(seatNum);
                chatterManager.positionList.Add(transform.position);

                //remove this object from the playerlist application variable by getting this object's name

                Destroy(gameObject);
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
