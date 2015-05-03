using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CoolCursor : MonoBehaviour
{
	public LayerMask Mask;

	Camera mainCamera;
	SpriteRenderer sr;

	[SerializeField]
	Sprite handClosed;
	[SerializeField]
	Sprite handOpen;
	[SerializeField]
	Sprite handServe;

	Vector3 handTargetPosition;

	void Start() {
		mainCamera = CameraMover.Instance.mainCamera;
		sr = GetComponent<SpriteRenderer> ();
	}

	void Update() {
		Rect screenRect = new Rect(0,0, Screen.width, Screen.height);
		if (!screenRect.Contains (Input.mousePosition)) {
			sr.enabled = false;
			return;
		}
		sr.enabled = true;
		
		var clickRay = mainCamera.ScreenPointToRay (Input.mousePosition);
		Debug.DrawRay(clickRay.origin, clickRay.direction * 500f);
		RaycastHit clickHit;
		if (Physics.Raycast(clickRay, out clickHit, 500f, Mask)) {
			handTargetPosition = clickHit.point;
			if(handTargetPosition.y > -1f) { handTargetPosition.y = -1f; }
			transform.position = handTargetPosition;
		}
		if (Input.GetMouseButton (0)) {
			sr.sprite = handClosed;
		} else {
			sr.sprite = handOpen;
		}
		Cursor.visible = false;

		// try a ray from the camera to the cursor position, see if it hits a dude
		Ray customerRay = new Ray(mainCamera.transform.position, Vector3.Normalize(transform.position - mainCamera.transform.position));
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(customerRay, out clickHit, 500f, LayerMask.GetMask("Customers")))
		{
			Customers.Instance.CustomerOuch();
		}
	}

	void OnDrawGizmos() {
		HingeJoint hj;
		if (hj = GetComponent<HingeJoint>())
		{
			Gizmos.DrawSphere(transform.position, 0.1f);
			//Gizmos.DrawSphere(transform.position + hj.anchor, 0.1f);
			Gizmos.DrawRay(transform.position, hj.axis);
		}
	}
}
