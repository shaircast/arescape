using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    void Update () {
        Vector3 randomDirection = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f) , Random.Range(-100.0f, 100.0f));
		transform.Rotate (randomDirection * Time.deltaTime);
	}
}
