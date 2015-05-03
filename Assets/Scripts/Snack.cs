using System.IO;
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Snack : MonoBehaviour {

	public string snackType;

	public void Start()
	{
		if (GetComponent<BoxCollider>() != null)
			Destroy(gameObject.GetComponent<BoxCollider>());
		gameObject.AddComponent<BoxCollider>();
		// TODO : resize based on sprite bounds?
	}

	public void Update() {
	}
}
