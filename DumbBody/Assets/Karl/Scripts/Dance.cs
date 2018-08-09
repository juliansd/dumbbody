using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dance : MonoBehaviour {

    private readonly float speed = 10.0f;
    private readonly float moveAngle = 45.0f;

    public static Dance instance = null;
    public GameObject[] joints;

    public Quaternion[] musicOffPositions;

    private bool musicPlaying = true;

    bool needsReset = false;

    public Transform body;

	void Awake () {
        instance = this; //Singleton
	}
    private void Start()
    {
        body = transform.Find("Torso");
        for(int i = 0; i < 4; i++)
        {
            musicOffPositions[i] = joints[i].transform.rotation;
        }
    }
    private void Update()
    {
        if(SequencerAudio_v1.count > 0)
        {
            MakeDance();
            needsReset = true;
        }
        else if(SequencerAudio_v1.count==0 && needsReset)
        {
            Debug.Log("Music is off");
            for (int i = 0; i < 4; i++)
            {
                joints[i].transform.rotation = musicOffPositions[i];
            }
            needsReset = false;
        }
    }

    public void MakeDance()
    {
        joints[0].transform.rotation = Quaternion.Euler(moveAngle * Mathf.Sin(Time.time * speed), moveAngle * Mathf.Sin(Time.time * speed), moveAngle * Mathf.Sin(Time.time * speed));
        joints[1].transform.rotation = Quaternion.Euler(moveAngle * Mathf.Sin(Time.time * speed), moveAngle * Mathf.Sin(Time.time * speed), moveAngle * Mathf.Sin(Time.time * speed));
        joints[2].transform.rotation = Quaternion.Euler(moveAngle * Mathf.Cos(Time.time * speed), moveAngle * Mathf.Sin(Time.time * speed),  0f);
        joints[3].transform.rotation = Quaternion.Euler(moveAngle * Mathf.Sin(Time.time * speed), moveAngle * Mathf.Sin(Time.time * speed), 0f);
    }
}
