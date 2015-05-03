using System.IO;
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Snack : MonoBehaviour {

	public string snackType;
	public bool placed;

	public void Start()
	{
		if (GetComponent<BoxCollider>() != null)
			Destroy(gameObject.GetComponent<BoxCollider>());
		gameObject.AddComponent<BoxCollider>();

		var size = GetComponent<BoxCollider>().size;
		size.z = 0.01f;
		GetComponent<BoxCollider>().size = size;

		if (GetComponent<Rigidbody>() != null)
			Destroy(gameObject.GetComponent<Rigidbody>());
		var rigidBody = gameObject.AddComponent<Rigidbody>();
		rigidBody.isKinematic = true;
		rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY |
		                        RigidbodyConstraints.FreezePositionZ;
	}

	public void Update() 
	{
	}
}
