using UnityEngine;
using System.Collections;

public class CookingMat : MonoBehaviour {

	[SerializeField]
	GameObject plateTemplate;

	void Start() {
		SpawnNewPlate();
	}

	public void RecieveSnack(Snack snack) {
		Debug.Log("Placed Snack " + snack.snackType);
		snack.placed = true;
		snack.transform.parent = gameObject.transform;
		//Vector3 snackPosition = snack.gameObject.transform.position;
	}

	public void SpawnNewPlate() {
	}
}
