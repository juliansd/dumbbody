using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/*
 * Script to handle control of the player. Manages the location of the camera, as well as whether a click is interpreted as
 * movement, or just a button click.
 * 
 * Attached to: Player prefab
 */ 

public class PlayerController : NetworkBehaviour {

    #region Variables

    [SyncVar]
    public Color playerColor;

    [HideInInspector]
    public const float MAXSPEED = 1.5f;

    [HideInInspector]
    public float speed = 0.0f;

    [HideInInspector]
    public Transform head;

    int moveTimer = 0;

    private Transform cam; //Store Camera.main.transform
    public Transform camParent; //Store CameraContainer


    SequencerStateStatus sequencerStateManager; //SequencerStateObject component
    CreateSequencerButtons sequencerButtonManager; //SequencerCanvas component
    #endregion Variables


    #region Methods


    public override void OnStartServer()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("SequencerState");
        if(temp != null)
        {
            sequencerStateManager = temp.GetComponent<SequencerStateStatus>();
        }

        GameObject temp_1 = GameObject.FindGameObjectWithTag("SequencerCanvas");
        if (temp_1 != null)
        {
            sequencerButtonManager = temp_1.GetComponent<CreateSequencerButtons>();
        }
        playerColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        CmdSetColor(playerColor);
    }

    public override void OnStartLocalPlayer () {
        playerColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        CmdSetColor(playerColor);
        head = transform.Find("Head");
        cam = Camera.main.transform;
        camParent = cam.parent;
        camParent.position = head.position;

        while(sequencerStateManager ==null)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("SequencerState");
            if (temp != null)
                sequencerStateManager = temp.GetComponent<SequencerStateStatus>();
        }
        while (sequencerButtonManager == null)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("SequencerCanvas");
            if (temp != null)
                sequencerButtonManager = temp.GetComponent<CreateSequencerButtons>();
        }

	}

    void Start()
    {
        if (isServer)
            RpcSetColor(playerColor);
    }

    void Update () {

        if (!isLocalPlayer)
            return;

        if (Input.GetMouseButton(0))
            ButtonHandler();

        camParent.position = head.position;


        Vector3 turn = cam.eulerAngles;
        turn.x = 0f;
        turn.z = 0f;
        transform.eulerAngles = turn;

	}

    public void ButtonHandler()
    {
        if (Input.GetMouseButton(0) && moveTimer > 20) // if mouse button is being held down for certain amount of time
        {
            if (Input.GetMouseButtonDown(0)) { moveTimer = 0; speed = 0.0f; return; } // if button is released, reset variables and exit function
            if (speed < MAXSPEED) // accellerate
            {
                camParent.position = head.position;
                Vector3 moveDirection = cam.forward.normalized; // direction to move player towards
                moveDirection.y = 0f; // do not change y-position
                transform.Translate(transform.InverseTransformDirection(moveDirection) * speed);
                speed += 0.005f; //increase speed

            }
            else
            { //if MAXSPEED reached, continue at that rate
                camParent.position = head.position;
                Vector3 moveDirection = cam.forward.normalized;
                moveDirection.y = 0f;
                transform.Translate(transform.InverseTransformDirection(moveDirection) * MAXSPEED);

            }
        }
        else
        {
            //if button not held down long enough, shoot projectile
            if (Input.GetMouseButtonDown(0))
            {
                moveTimer = 0; // reset time
            }
            else
                moveTimer++; //otherwise add time if button is being held down
        }
    }
    [ClientRpc]
    public void RpcSetColor(Color color)
    {
        playerColor = color;
        transform.Find("Torso").GetComponent<Renderer>().material.color = color;
    }

    [Command]
    public void CmdSetColor(Color color)
    {
        playerColor = color;
    }
    
    #endregion Methods
}
