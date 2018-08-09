using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Simple script to add dance-like movement to players when music is playing
 * 
 * Attached to: Player prefab
 */ 

public class Dance : MonoBehaviour {

    #region Variables
    private readonly float speed = 10.0f;
    private readonly float moveAngle = 45.0f;

    public GameObject[] joints; //all the joints of player

    public Quaternion[] musicOffPositions; //original limb positions

    bool needsReset = false; //keeps track of if the players limbs have been moving, 
                                //and whether they need to be reset to original position when music is off
    #endregion Variables


    #region Methods

    private void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            musicOffPositions[i] = joints[i].transform.rotation; // get all original positions of player's joints
        }
    }
    private void Update()
    {

        if(SequencerAudio_v1.count > 0) // if music is on
        {
            MakeDance(); // dance
            needsReset = true;
        }
        else if(SequencerAudio_v1.count==0 && needsReset) // if music is not on, and player limbs need to be reset
        {
            Debug.Log("Music is off");
            for (int i = 0; i < 4; i++)
            {
                joints[i].transform.rotation = musicOffPositions[i];
            }
            needsReset = false;
        }
    }

    //Takes all of the joints of the player, and makes the corresponding limbs move
    public void MakeDance()
    {
        joints[0].transform.rotation = Quaternion.Euler(moveAngle * Mathf.Sin(Time.time * speed), moveAngle * Mathf.Sin(Time.time * speed), moveAngle * Mathf.Sin(Time.time * speed));
        joints[1].transform.rotation = Quaternion.Euler(moveAngle * Mathf.Sin(Time.time * speed), moveAngle * Mathf.Sin(Time.time * speed), moveAngle * Mathf.Sin(Time.time * speed));
        joints[2].transform.rotation = Quaternion.Euler(moveAngle * Mathf.Cos(Time.time * speed), moveAngle * Mathf.Sin(Time.time * speed),  0f);
        joints[3].transform.rotation = Quaternion.Euler(moveAngle * Mathf.Sin(Time.time * speed), moveAngle * Mathf.Sin(Time.time * speed), 0f);
    }
    #endregion Methods
}
