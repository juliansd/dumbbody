using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
 * This script is designed to instantiate glowing objects to add to the scene for lighting.
 * 
 * Attached to: LightController object
 */ 

public class CreateLumiere : MonoBehaviour {

    public static CreateLumiere instance = null;

    [SerializeField]
    private GameObject[] lumieres;

    [SerializeField]
    private int numLumieres;

    private void Awake()
    {
        instance = this; //Singleton
    }

    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 10.0f); // Rotate the lights to add atmosphere
    }

    public void CreateMoodLights() {
		/*
         * // Create randomly distributed lumieres
        for(int i = 0; i < numLumieres; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-50f, 50f), 0.5f, Random.Range(-50f, 50f));
            GameObject lumiere = (GameObject)Instantiate(lumieres[i%5], pos, Quaternion.identity);
        }*/

        // Create evenly distributed rows and columns of lumieres
        for(int i =0; i < 10; i++)
            for(int j = 0; j < 10; j++)
            {
                Vector3 pos = new Vector3(-40 + j*10f, 12f, -40 +i *10f);
                GameObject lumiere = (GameObject)Instantiate(lumieres[j % 5], pos, Quaternion.identity);
                lumiere.transform.parent = transform;
            }
	}
}
