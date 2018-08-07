using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

/*
 * Detects button pressed by getting the information from CreateSequencerButtons.instance.cs GetButtonName()
 * 
 * Attached to Player prefab
 */ 
public class DetectButtons : NetworkBehaviour {

    SequencerStateStatus sequencerStateManager;

    private ulong bitToFlip = 0x00000000; // intialize to 0 (64-bit)

    [SyncVar]
    private bool isClicked = false;

    void Start()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("SequencerState");
        if (temp != null)
        {
            sequencerStateManager = temp.GetComponent<SequencerStateStatus>();
        }

       
    }
    /*
    public override void OnStartClient()
    {
        base.OnStartClient();
    }
    */
    public override void OnStartLocalPlayer()
    {
        while (sequencerStateManager == null)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("SequencerState");
            if (temp != null)
                sequencerStateManager = temp.GetComponent<SequencerStateStatus>();
        }

        
        bitToFlip = sequencerStateManager.bitsOnOrOff;
    }

    void Update()
    {
        int buttonNum = CreateSequencerButtons.instance.buttonNumber;
        if (!isLocalPlayer)
        {
            Debug.Log("Not Local Player");
            return;
        }

        isClicked = CreateSequencerButtons.instance.clicked;

        Debug.Log("clicked = " + isClicked);
        if (isClicked)
        {
            Debug.Log("Button Name in CMD: " + buttonNum);
            int shiftPos = buttonNum; // buttonNumber is an integer between 0 and 63
            bitToFlip ^= 1ul << shiftPos; // Flip the bit at corresponding position of button
            Debug.Log(bitToFlip);
            CreateSequencerButtons.instance.clicked = false;
            CmdSendButtonData(bitToFlip); //Call command
            bitToFlip = 0x00000000;
        }
            
    }


    // Command sent to SequencerStateObject
    [Command]
    public void CmdSendButtonData(ulong bits)
    {
        //Debug.Log("Clicked = " + CreateSequencerButtons.instance.clicked);

        sequencerStateManager.UpdateBits(bits); //call Update bits function in SequencerStateStatus.cs
        isClicked = false; //set clicked back to false
    }
}
