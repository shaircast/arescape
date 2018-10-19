using System.Collections;
using System.Collections.Generic;
using GoogleARCore.Examples.HelloAR;
using UnityEngine;

public class BallController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("end"))
		{
			GameObject.Find("Example Controller").GetComponent<HelloARController>().state = 8;
			Destroy(gameObject);
		}
	}
}
