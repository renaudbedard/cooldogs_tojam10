using System.Linq;
using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour {

	float rotationAmount;
	float rotationSpeed;

	public AudioClip[] TalkAudios;
	public AudioClip PainAudio;

	int lastTalkFrame;
	public Sprite[] TalkFrames;
	public Sprite IdleFrame;

	public bool LikesGarbage;
	bool talking;
	float sinceChangedSprite;

	SpriteRenderer spriteRenderer;
	AudioSource audioSource;

	public float TalkChangeSpriteSpeed;

	void Start() {
		rotationAmount = UnityEngine.Random.Range (0f, 6f);
		rotationSpeed = UnityEngine.Random.Range (2f, 10f);
		transform.RotateAround(transform.position, Vector3.forward, rotationAmount * -1f);
		iTween.RotateTo (gameObject, iTween.Hash ("z", rotationAmount, 
		                                          "speed", rotationSpeed,
		                                          "loopType", "pingPong",
		                                          "easeType", "easeInOutQuad"));

		spriteRenderer = GetComponent<SpriteRenderer>();
		audioSource = GetComponent<AudioSource>();

		spriteRenderer.sprite = IdleFrame;
	}

	void Update()
	{
		if (talking)
		{
			sinceChangedSprite += Time.deltaTime;
			if (sinceChangedSprite > TalkChangeSpriteSpeed)
			{
				sinceChangedSprite -= TalkChangeSpriteSpeed;

				lastTalkFrame++;
				if (lastTalkFrame >= TalkFrames.Length)
					lastTalkFrame = 0;
				spriteRenderer.sprite = TalkFrames[lastTalkFrame];
			}

			if (!audioSource.isPlaying)
			{
				talking = false;
				spriteRenderer.sprite = IdleFrame;
			}
		}
	}

	public void Talk()
	{
		audioSource.pitch = Random.Range(0.95f, 1.05f);
		audioSource.clip = TalkAudios.Shuffle().First();
		audioSource.Play();
		talking = true;
	}

	public void Ouch()
	{
		audioSource.pitch = Random.Range(0.9f, 1.1f);
		audioSource.PlayOneShot(PainAudio);

		StartCoroutine(SwitchFrame());
	}

	IEnumerator SwitchFrame()
	{
		spriteRenderer.sprite = TalkFrames.Shuffle().First();
		yield return new WaitForSeconds(0.25f);
		spriteRenderer.sprite = IdleFrame;
	}
}
