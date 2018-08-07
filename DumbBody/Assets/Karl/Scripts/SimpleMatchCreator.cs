﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class SimpleMatchCreator : MonoBehaviour {

    #region Variables

    public Canvas networkCanvas;
    public Canvas melodyCanvas;
    public Canvas generatedSequencerCanvas;
    public GameObject directionalLight;
    #endregion Variables


    #region Methods
    // create singleton of match maker object
    void Start()
    {
        melodyCanvas.enabled = false;
        generatedSequencerCanvas.enabled = false;
        networkCanvas.enabled = true; // Ensure that menu buttons are visible
        NetworkManager.singleton.StartMatchMaker(); //Start matchmaker
    }

    // Create new internet match
    public void CreateInternetMatch(string name)
    {
        NetworkManager.singleton.matchMaker.CreateMatch(name, 4, true, "", "", "", 0, 0, OnInternetMatchCreate);
        networkCanvas.enabled = false; // disables menu buttons when matched is created on client/host
        //melodyCanvas.enabled = true;
        generatedSequencerCanvas.enabled = true;
        CreateLumiere.instance.CreateMoodLights();
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3(-17f, -30f, 0f));
    }

    //Callback needed for CreateMatch() function
    private void OnInternetMatchCreate(bool created, string extendedInfo, MatchInfo info)
    {
        if (created)
        {
            MatchInfo hostInfo = info;
            NetworkServer.Listen(hostInfo, 9000);
            NetworkManager.singleton.StartHost(hostInfo);
        }
        else
        {
            Debug.Log("Match failed to create");
        }
    }

    // To join an already created internet match
    public void FindInternetMatch(string name)
    {
        NetworkManager.singleton.matchMaker.ListMatches(0, 10, name, true, 0, 0, OnInternetMatchList);
        networkCanvas.enabled = false; //disable button menu when match found
        //melodyCanvas.enabled = true;
        generatedSequencerCanvas.enabled = true;
        CreateLumiere.instance.CreateMoodLights();
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3(-17f, -30f, 0f));
    }

    //Callback needed for ListMatches() function
    private void OnInternetMatchList(bool created, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (created)
        {
            if (matches.Count != 0)
            {
                NetworkManager.singleton.matchMaker.JoinMatch(matches[matches.Count - 1].networkId, "", "", "", 0, 0, OnJoinInternetMatch);
            }
            else
            {
                Debug.Log("No matches in requested room");
            }
        }
        else
        {
            Debug.LogError("Could not connect to a matchmaker");
        }
    }

    //Callback needed for JoinMatch() function
    private void OnJoinInternetMatch(bool created, string extendedInfo, MatchInfo info)
    {
        if (created)
        {
            MatchInfo hostInfo = info;
            NetworkManager.singleton.StartClient(hostInfo); //Start client in match
        }
        else
        {
            Debug.LogError("Match join failed");
        }
    }
    #endregion Methods
}
