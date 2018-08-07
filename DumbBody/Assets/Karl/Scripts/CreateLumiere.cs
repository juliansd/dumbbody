using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateLumiere : MonoBehaviour {

    public static CreateLumiere instance = null;

    [SerializeField]
    private GameObject[] lumieres;

    [SerializeField]
    private int numLumieres;

    private void Awake()
    {
        instance = this;
    }

    public void CreateMoodLights() {
		
        for(int i = 0; i < numLumieres; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-50f, 50f), 0.5f, Random.Range(-50f, 50f));
            GameObject lumiere = (GameObject)Instantiate(lumieres[i%5], pos, Quaternion.identity);
        }
	}
}
