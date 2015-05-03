using UnityEngine;
using System.Collections;

public class CookingMat : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RecieveSnack(Snack snack) {
		Debug.Log("Placed Snack " + snack.snackType);
		snack.placed = true;
		//Vector3 snackPosition = snack.gameObject.transform.position;
	}
}
