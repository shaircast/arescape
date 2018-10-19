using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RosettaStoneController : MonoBehaviour {

	public AnimationCurve myCurve;
	
	void Start() {
		// 현재 위치를 받아와서 비석의 위치를 설정한다.
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		transform.position = movement;
	}

	void Update() {
		// 비석의 위치가 결정된 후, AnimationCurve인 myCurve에 따라 위 아래로 움직인다.
		transform.position = new Vector3(transform.position.x, myCurve.Evaluate((Time.time % myCurve.length)), transform.position.z);
	}

}
