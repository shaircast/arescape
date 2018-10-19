using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    void Update () {
        transform.Rotate (new Vector3 (Random.Range(-10.0f, 10.0f), 30 , Random.Range(-10.0f, 10.0f)) * Time.deltaTime);
	}
}