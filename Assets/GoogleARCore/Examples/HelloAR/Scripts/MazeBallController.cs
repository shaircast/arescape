using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MazeBallController : MonoBehaviour {

	public float speed;
	public Text winText;

	private Rigidbody rb;

	void Start() {
		rb = GetComponent<Rigidbody>();
		rb.useGravity = true;
	}

	void FixedUpdate() {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		rb.AddForce(movement * speed);
		
	}

	void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Exit")){
			winText.text = "You Succeeded!";
		}
    }
	
}
