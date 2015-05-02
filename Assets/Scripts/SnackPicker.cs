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

	public LayerMask dropSnackLayers;
	public LayerMask heldSnacksLayer;
	public LayerMask snacksOnly;


	// Use this for initialization
	void Start () {
		collider = gameObject.GetComponent<Collider> ();
		camera = gameObject.GetComponent<Camera> ();
		snackPrefab = Resources.Load("SnackPrefab") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		clickRay = camera.ScreenPointToRay (Input.mousePosition);
		if (Input.GetMouseButtonDown (0)) {
			Debug.DrawRay (transform.position + transform.forward, clickRay.direction * 500f);
			if (heldSnack == null) {
				if (Physics.Raycast (clickRay, out clickHit, 500f, snacksOnly)) {
					Debug.Log (clickHit.collider.gameObject.name);
					selectedSnack = clickHit.collider.GetComponent<Snack>();
					if (selectedSnack) {
						heldSnack = selectedSnack;
					}
				}
			}
		} else if (Input.GetMouseButtonUp (0) && heldSnack != null) {
			if (Physics.Raycast (clickRay, out clickHit, 500f, dropSnackLayers)) {
				cookingMat = clickHit.collider.GetComponent<CookingMat>();
				if (cookingMat != null) {
					if (heldSnack != null) {
						cookingMat.RecieveSnack(heldSnack);
					}
				}
				heldSnack = null;
			}
		}

		if (heldSnack != null) {
			Physics.Raycast (clickRay, out clickHit, 500f, heldSnacksLayer);
			heldSnack.transform.position = clickHit.point;
		}
	}
}
