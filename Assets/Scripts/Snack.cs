using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Snack : MonoBehaviour {

	public string snackType;
	public bool placed;

	Transform spriteContainer;

	public AudioClip[] Foley;
	AudioClip Pick, Drop;

	public void Initialize()
	{
		spriteContainer = transform.FindChild("spriteContainer");

		if (spriteContainer.GetComponent<BoxCollider>() != null)
			Destroy(spriteContainer.GetComponent<BoxCollider>());
		spriteContainer.gameObject.AddComponent<BoxCollider>();

		var size = spriteContainer.GetComponent<BoxCollider>().size;
		size.z = 0.01f;
		spriteContainer.GetComponent<BoxCollider>().size = size;

		if (spriteContainer.GetComponent<Rigidbody>() != null)
			Destroy(spriteContainer.GetComponent<Rigidbody>());
		var rigidBody = spriteContainer.gameObject.AddComponent<Rigidbody>();
		rigidBody.isKinematic = true;
		rigidBody.angularDrag = 1;
		rigidBody.mass = 100;

		Pick = Foley.Shuffle().First();
		Drop = Foley.Except(new [] {Pick}).Shuffle().First();

		//rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
	}

	public void PlayFoley(bool pick)
	{
		GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
		GetComponent<AudioSource>().PlayOneShot(pick ? Pick : Drop);
	}

}
