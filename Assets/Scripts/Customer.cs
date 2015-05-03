using System.Linq;
using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour {

	float rotationAmount;
	float rotationSpeed;

	public AudioClip[] TalkAudios;
	public AudioClip PainAudio;

	public bool LikesGarbage;

	void Start() {
		rotationAmount = UnityEngine.Random.Range (0f, 6f);
		rotationSpeed = UnityEngine.Random.Range (2f, 10f);
		transform.RotateAround(transform.position, Vector3.forward, rotationAmount * -1f);
		iTween.RotateTo (gameObject, iTween.Hash ("z", rotationAmount, 
		                                          "speed", rotationSpeed,
		                                          "loopType", "pingPong",
		                                          "easeType", "easeInOutQuad"));
	}

	public void Update()
	{
		
	}

	public void Talk()
	{
		GetComponent<AudioSource>().PlayOneShot(TalkAudios.Shuffle().First());
	}

	public void Ouch()
	{
		GetComponent<AudioSource>().PlayOneShot(PainAudio);
	}
}
