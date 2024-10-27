using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour {

    public float destroyDelay = 10f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        destroyDelay -= Time.deltaTime;

        if (destroyDelay <= 0)
        {
            Destroy(gameObject);
        }
	}
}
