using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dance : MonoBehaviour {

    private readonly float speed = 10.0f;
    private readonly float moveAngle = 15.0f;

    public static Dance instance = null;
    public GameObject[] joints;

    private bool musicPlaying = true;

	void Awake () {
        instance = this; //Singleton
	}

    private void Update()
    {
        if(SequencerAudio_v1.count > 0)
        {
            MakeDance();
        }
    }

    public void MakeDance()
    {
        joints[0].transform.rotation = Quaternion.Euler(moveAngle * Mathf.Sin(Time.time * speed), 0f, moveAngle * Mathf.Sin(Time.time * speed));
        joints[1].transform.rotation = Quaternion.Euler(moveAngle * Mathf.Sin(Time.time * speed),0f, moveAngle * Mathf.Cos(Time.time * speed));
        joints[2].transform.rotation = Quaternion.Euler((moveAngle*3) * Mathf.Cos(Time.time * speed), (moveAngle * 3) * Mathf.Sin(Time.time * speed),  0f);
        joints[3].transform.rotation = Quaternion.Euler((moveAngle * 3) * Mathf.Sin(Time.time * speed), (moveAngle * 3) * Mathf.Sin(Time.time * speed), 0f);
    }
}
