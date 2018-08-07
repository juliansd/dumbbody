using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
 * This script programatically creates a grid of buttons attached to a canvas which will used as the sequencer. The function
 * GetButtonName() is also called whenever a user clicks a button on the sequencer.
 * 
 * Attached to: SequencerCanvas
 */ 

public class CreateSequencerButtons : MonoBehaviour {

    #region Variables

    public static CreateSequencerButtons instance = null; //Singleton

    [SerializeField]
    private Transform sequencerGrid; //SequencerButtonField child of SequencerCanvas

    [SerializeField]
    private GameObject buttonPrefab; //Button prefab to clone for use in sequencer

    public static int sizeI = 8; // number of rows of sequencer

    public static int sizeJ = 8; //number of columns of sequencer

    public  GameObject[,] buttons; //Stores all button GameObjects for access later

    public int buttonNumber;

    public bool clicked;

    #endregion Variables


    #region Methods

    private void Awake()
    {
        instance = this; //Singleton
    }

    void Start()
    {

        //Creates buttons for sequencer (8x8 grid)
        buttons = new GameObject[sizeI, sizeJ];
        int buttonCount = 0;
        for (int i = 0; i < sizeI; i++)
        {
            for (int j = 0; j < sizeJ; j++)
            {
                GameObject button = (GameObject)Instantiate(buttonPrefab);
                button.transform.SetParent(sequencerGrid, false);
                button.name = "" + buttonCount;
                button.GetComponent<Button>().onClick.AddListener(GetButtonName); //onClick listener to get button name for use in DetectButtons.cs
                button.SetActive(true);
                buttons[i, j] = button; // store buttons in 2D array for access to change colors in SequencerAudio_v1.cs
                buttonCount++;
            }
        }
        
    }

    // Used to get button name that is then passed to the Command in DetectButtons.cs
    public void GetButtonName()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name; //EventSystem returns name of button that will be a string number between 0-63
        buttonNumber = int.Parse(buttonName); //Convert button name to int and set to buttonNumber variable in script
        clicked = true; //set to true as indicator to use in Update() in DetectButtons.cs
    }


    #endregion Methods
}
