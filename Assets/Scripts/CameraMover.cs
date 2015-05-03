using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour {

	public bool LookingAtWindow = true;
	public float lookSpeed = 5f;
	Quaternion WindowTargetRotation = new Quaternion(0f,0f,0f,1f);
	Quaternion CounterTargetRotation = new Quaternion(0.4f,0f,0f,0.9f);
	Quaternion targetRotation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (LookingAtWindow) {
			targetRotation = WindowTargetRotation;
		} else {
			targetRotation = CounterTargetRotation;
		}

		//animationTargetDirection = Vector3.RotateTowards(transform.rotation, targetDirection, lookSpeed * Time.deltaTime, 0.0F);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
	}

	public void ToggleMode() {
		LookingAtWindow = !LookingAtWindow;
	}
}
