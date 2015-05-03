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

	public LayerMask dropSnackLayers;
	public LayerMask heldSnacksLayer;
	public LayerMask snacksOnly;

	public GameObject Cursor;


	// Use this for initialization
	void Start () {
		collider = gameObject.GetComponent<Collider> ();
		camera = gameObject.GetComponent<Camera> ();
		snackPrefab = Resources.Load("SnackPrefab") as GameObject;
	}

	public void Reset()
	{
		lastLayer = 0;
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
					if (selectedSnack && !selectedSnack.placed) {
						heldSnack = selectedSnack;
						heldSnack.GetComponent<SpriteRenderer>().sortingOrder = lastLayer + 1;
						holdOffset = heldSnack.transform.position - clickHit.point;
						heldSnack.GetComponent<Rigidbody>().isKinematic = false;
					}
				}
			}
		}
		if (Input.GetMouseButtonUp (0) && heldSnack != null) {
			if (Physics.Raycast (clickRay, out clickHit, 500f, dropSnackLayers)) {
				cookingMat = clickHit.collider.GetComponent<CookingMat>();
				if (cookingMat != null) {
					if (heldSnack != null) {
						cookingMat.RecieveSnack(heldSnack);
					}
				}
			}

			heldSnack.GetComponent<SpriteRenderer>().sortingOrder = lastLayer;
			lastLayer++;
			heldSnack.GetComponent<Rigidbody>().isKinematic = true;
			heldSnack = null;
		}

		if (heldSnack != null) {
			Physics.Raycast (clickRay, out clickHit, 500f, heldSnacksLayer);
			heldSnack.transform.position = clickHit.point + holdOffset;
		}
	}
}
