using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Customers : MonoBehaviour {

	public static Customers Instance { private set; get; }

	public bool doingThings = false;
	
	Customer currentCustomer;
	iTweenPath currentSpawnPath;

	public Sprite[] Sprites;
	private iTweenPath[] SpawnPaths;
	
	[SerializeField]
	Transform windowCenter;

	[SerializeField]
	GameObject CustomerTemplate;
	
	void Awake() {
		Instance = this;
		DontDestroyOnLoad(gameObject);

		SpawnPaths = GetComponents<iTweenPath>();
	}

	public void SpawnNextCustomer() {
		doingThings = true;

		DestroyCurrentCustomer();

		currentSpawnPath = SpawnPaths.Shuffle ().First ();
		
		var obj = Instantiate(CustomerTemplate, currentSpawnPath.nodes.First(), Quaternion.identity) as GameObject;
		currentCustomer = obj.GetComponent<Customer>();

		iTween.MoveTo(currentCustomer.gameObject, iTween.Hash("path", currentSpawnPath.nodes.ToArray(),
		                                                      "easeType", "easeOutQuad",
		                                                      "time", 6f,
		                                                      "oncomplete", "CustomerEnterFinish",
		                                                      "onCompleteTarget", gameObject));
	}
	
	void CustomerEnterFinish() {
		Flow.CurrentPhase = Flow.Phase.CustomerSpeech;
		doingThings = false;
	}

	public void DestroyCurrentCustomer() {
		if (currentCustomer != null) {
			Destroy (currentCustomer.gameObject);
		}
	}
}
