using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour {

	float rotationAmount;
	float rotationSpeed;

	void Start() {
		rotationAmount = UnityEngine.Random.Range (0f, 6f);
		rotationSpeed = UnityEngine.Random.Range (2f, 10f);
		transform.RotateAround(transform.position, Vector3.forward, rotationAmount * -1f);
		iTween.RotateTo (gameObject, iTween.Hash ("z", rotationAmount, 
		                                          "speed", rotationSpeed,
		                                          "loopType", "pingPong",
		                                          "easeType", "easeInOutQuad"));
	}
}
