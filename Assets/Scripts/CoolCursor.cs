using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CoolCursor : MonoBehaviour {

	public static CoolCursor Instance { private set; get; }
	
	public LayerMask Mask;

	Camera mainCamera;
	SpriteRenderer sr;

	[SerializeField]
	Sprite handClosed;
	[SerializeField]
	Sprite handOpen;
	[SerializeField]
	Sprite handServe;

	public Vector3 handTargetPosition;
	Vector3 baseScale;

	public bool hugCursor;
	public bool hugTarget;

	void Awake() {
		Instance = this;
		DontDestroyOnLoad(gameObject);

		mainCamera = CameraMover.Instance.mainCamera;
		sr = GetComponent<SpriteRenderer> ();
		baseScale = gameObject.transform.localScale;
		handTargetPosition = gameObject.transform.position;
		hugCursor = false;
		hugTarget = true;
	}

	void Update() {
		Rect screenRect = new Rect(0,0, Screen.width, Screen.height);
		if (!screenRect.Contains (Input.mousePosition)) {
			Cursor.visible = true;
			return;
		}
		Cursor.visible = false;
		RaycastHit clickHit;
		if (hugCursor == true)
		{
			var clickRay = mainCamera.ScreenPointToRay (Input.mousePosition);
			Debug.DrawRay (clickRay.origin, clickRay.direction * 500f);
			if (Physics.Raycast (clickRay, out clickHit, 500f, Mask)) {
				handTargetPosition = clickHit.point;
			}
			if (Input.GetMouseButton (0)) {
				SetSprite("closed");
			} else {
				SetSprite("open");
			}
		}

		if (handTargetPosition.y > -1f && hugCursor) {
			handTargetPosition.y = -1f;
		}
		if (hugTarget) {
			transform.position += (handTargetPosition - transform.position) * Time.deltaTime * 20f;
		}

		// try a ray from the camera to the cursor position, see if it hits a dude
		Ray customerRay = new Ray(mainCamera.transform.position, Vector3.Normalize(transform.position - mainCamera.transform.position));
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(customerRay, out clickHit, 500f, LayerMask.GetMask("Customers"))) {
			Customers.Instance.CustomerOuch();
		}
	}

	public void SetSprite(string mode) {
		switch (mode) {
		case "serve":
			sr.sprite = handServe;
			sr.sortingOrder = -2;
			gameObject.transform.localScale = baseScale;
			break;
		case "closed":
			sr.sprite = handClosed;
			sr.sortingOrder = 1000;
			gameObject.transform.localScale = baseScale;
			break;
		default:
			sr.sprite = handOpen;
			sr.sortingOrder = 1000;
			gameObject.transform.localScale = baseScale * 0.9f;
			break;
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
