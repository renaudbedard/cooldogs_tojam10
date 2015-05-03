using UnityEngine;
using System.Collections;

public class CookingMat : MonoBehaviour {

	[SerializeField]
	GameObject plateTemplate;

	public static GameObject currentPlate;

	Vector3 plateOffset;

	void Start() {
		plateOffset = new Vector3(0f, -0.8f, 0f);
		SpawnNewPlate();
	}

	public void RecieveSnack(Snack snack) {
		Debug.Log("Placed Snack " + snack.snackType);
		snack.placed = true;
		snack.transform.parent = currentPlate.transform;
		//Vector3 snackPosition = snack.gameObject.transform.position;
	}

	public void SpawnNewPlate() {
		if (currentPlate != null) {
			Destroy(currentPlate);
		}
		currentPlate = (GameObject)Instantiate (plateTemplate, transform.position, transform.rotation);
		currentPlate.transform.Translate(plateOffset);
		//currentPlate.transform.parent = gameObject.transform;
	}
}
