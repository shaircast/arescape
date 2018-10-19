using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MazeBallController : MonoBehaviour {

	public float speed;
	public Text winText;

	private Rigidbody rb;

	void Start() {
		rb = GetComponent<Rigidbody>();
		// 중력을 사용한다
		rb.useGravity = true;
	}

	void FixedUpdate() {
		// 공의 움직임을 받아서 speed에 따라 이동시킨다
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		rb.AddForce(movement * speed);
		
	}

	void OnTriggerEnter(Collider other) {
		// Exit에 도착하면 성공했다는 메시지를 띄워준다
        if(other.gameObject.CompareTag("Exit")){
			winText.text = "You Succeeded!";
		}
    }
	
}
