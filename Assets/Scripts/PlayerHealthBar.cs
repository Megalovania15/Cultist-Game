using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour {

    public Transform target;

    public float offset;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        FollowPlayer();
	}

    void FollowPlayer()
    {
        var wantedPos = Camera.main.WorldToScreenPoint(target.position);

        //var wantedPos = Camera.main.WorldToViewportPoint(target.position);

        transform.position = wantedPos + new Vector3(0, offset, 0);
    }
}
