using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject target;

	public float dampTime;

	private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () 
	{
        
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		Vector3 playerPos = target.transform.position;

		Vector3 cameraPos = new Vector3 (0f, 0f, transform.position.z);

		Vector3 destination = playerPos + cameraPos;
        transform.position = Vector3.SmoothDamp (transform.position, destination, ref velocity, dampTime);

        //transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);

	}
}
