using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitationParticle : MonoBehaviour {

    public GameObject owner;

    private PlayerController pc;

	// Use this for initialization
	void Start ()
    {
        pc = owner.GetComponent<PlayerController>();
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (pc.child != null)
            transform.position = new Vector3 (pc.child.transform.position.x, pc.child.transform.position.y - 0.5f, 
                pc.child.transform.position.z);

    }
}
