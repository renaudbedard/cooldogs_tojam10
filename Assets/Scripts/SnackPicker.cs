using UnityEngine;
using System.Collections;

public class SnackPicker : MonoBehaviour {

	Ray clickRay;
	RaycastHit clickHit;
	Collider collider;
	Camera camera;

	CookingMat cookingMat;
	Snack selectedSnack;

	GameObject snackPrefab;
	Snack heldSnack;
	int lastLayer;
	Vector3 holdOffset;

	public bool hasSnacksOnPlate = false;

	public LayerMask dropSnackLayers;
	public LayerMask heldSnacksLayer;
	public LayerMask snacksOnly;

	public GameObject Cursor;
	HingeJoint lastJoint;


	// Use this for initialization
	void Start () {
		collider = gameObject.GetComponent<Collider> ();
		camera = gameObject.GetComponent<Camera> ();
		snackPrefab = Resources.Load("SnackPrefab") as GameObject;
	}

	public void Reset() {
		lastLayer = 0;
		hasSnacksOnPlate = false;
	}
	
	// Update is called once per frame
	void Update () {
		clickRay = camera.ScreenPointToRay (Input.mousePosition);
		if (Input.GetMouseButtonDown (0)) {
			Debug.DrawRay (transform.position + transform.forward, clickRay.direction * 500f);
			if (heldSnack == null) {
				if (Physics.Raycast (clickRay, out clickHit, 500f, snacksOnly)) {
					Debug.Log (clickHit.collider.gameObject.name);
					selectedSnack = clickHit.collider.transform.parent.GetComponent<Snack>();
					if (selectedSnack && !selectedSnack.placed) {
						heldSnack = selectedSnack;
						heldSnack.GetComponentInChildren<SpriteRenderer>().sortingOrder = lastLayer + 1;
						holdOffset = heldSnack.transform.position - clickHit.point;

						Cursor.transform.position = clickHit.point;

						var snackRB = heldSnack.GetComponentInChildren<Rigidbody>();
						snackRB.isKinematic = false;
						lastJoint = Cursor.AddComponent<HingeJoint>();
						lastJoint.axis = new Vector3(0, 0, 1);
						//lastJoint.autoConfigureConnectedAnchor = false;
						lastJoint.connectedBody = snackRB;

						lastJoint.anchor -= new Vector3(0f,1.5f,0f);
					}
				}
			}
		}
		if (Input.GetMouseButtonUp (0) && heldSnack != null) {

			heldSnack.GetComponentInChildren<SpriteRenderer>().sortingOrder = lastLayer;
			lastLayer++;
			heldSnack.GetComponentInChildren<Rigidbody>().isKinematic = true;
			heldSnack.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
			heldSnack.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;

			if (lastJoint)
			{
				Destroy(lastJoint);
				lastJoint = null;
			}

			if (Physics.Raycast (clickRay, out clickHit, 500f, dropSnackLayers)) {
				cookingMat = clickHit.collider.GetComponent<CookingMat>();
				if (cookingMat != null) {
					if (heldSnack != null) {
						cookingMat.RecieveSnack(heldSnack);
						hasSnacksOnPlate = true;
					}
				}
			}

			heldSnack = null;
		}

		if (heldSnack != null) {
			Physics.Raycast (clickRay, out clickHit, 500f, heldSnacksLayer);
			Cursor.transform.position = clickHit.point;
		}
	}
}
