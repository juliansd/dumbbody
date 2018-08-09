using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SequencerAudio_v1 : NetworkBehaviour {

    #region Variables

    private static int currentRow = 0; // Used in PlayMusic() to keep track of current button row being played

    public AudioSource[] srcAudio; //array of audio sources that will be played when chosen

    CreateSequencerButtons sequencerCanvas;
    SequencerStateStatus sequencerState; // keeps track of sequencer state

    public static int count = 0;


    #endregion Variables

    #region Methods

    public override void OnStartServer()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("SequencerState");
        if (temp != null)
        {
            sequencerState = temp.GetComponent<SequencerStateStatus>();
        }

        GameObject temp_0 = GameObject.FindGameObjectWithTag("SequencerCanvas");
        if (temp != null)
        {
            sequencerCanvas = temp_0.GetComponent<CreateSequencerButtons>();
        }
    }

    public override void OnStartLocalPlayer()
    {
        var audioController = GameObject.FindGameObjectWithTag("AudioController");
        for(int i = 0; i < audioController.transform.childCount; i++)
        {
            srcAudio[i] = audioController.transform.GetChild(i).GetComponent<AudioSource>();
        }

        while (sequencerState == null)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("SequencerState");
            if (temp != null)
                sequencerState = temp.GetComponent<SequencerStateStatus>();
        }

        while (sequencerCanvas == null)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("SequencerCanvas");
            if (temp != null)
                sequencerCanvas = temp.GetComponent<CreateSequencerButtons>();
        }

        InvokeRepeating("PlayMusic", 0.0f, 0.25f);
    }


    //Plays music
    public void PlayMusic()
    {
        count = 0; // if count > 0, this will indicate that the player needs to "dance"
        //Iterate through the ulong to see which buttons need to be "activated" on sequencer
        for(int k = 0; k < 64; k++)
        {
            if((sequencerState.bitsOnOrOff >>k & 1)== 1)
            {
                sequencerCanvas.buttons[k / 8, k % 8].GetComponent<Image>().color = Color.blue;
                count++;
            }
            else
            {
                sequencerCanvas.buttons[k / 8, k % 8].GetComponent<Image>().color = Color.white;
            }
        }

        

        int i = currentRow;
        print("sequencer onoff: " + sequencerState.bitsOnOrOff);
        print("On/Off value: " + (sequencerState.bitsOnOrOff >> (i * 8)));
        ulong toTurn = sequencerState.bitsOnOrOff >> (i * 8); // Check 8-bits each iteration
        //ulong toTurn = sequencerInfo.bitsOnOrOff >> (i * 8);

        for (int j = 0; j < 8; j++)
        {
            //Debug.Log("j: " + j + "To turn: " + ((toTurn >> j & 1) == 1));
           if ((toTurn >> j & 1) == 1)
                srcAudio[j].Play();
            

            //Change color of rows, row-by-row
            Image buttonImg = sequencerCanvas.buttons[i, j].GetComponent<Image>();
            if (buttonImg.color.Equals(Color.white) && !buttonImg.color.Equals(Color.blue))
                buttonImg.color = Color.yellow;
            else if (!buttonImg.color.Equals(Color.blue))
                buttonImg.color = Color.white;

        }
        currentRow++; //increment to next row
        if (currentRow > 7) currentRow = 0; //if at last row, set to 0
    }
    #endregion Methods
}
