using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Customers : MonoBehaviour {

	public static Customers Instance { private set; get; }

	public bool doingThings = false;
	
	Customer currentCustomer;

	public Sprite[] Sprites;
	private CustomerSpawn[] SpawnPoints;
	
	[SerializeField]
	Transform windowCenter;

	[SerializeField]
	GameObject CustomerTemplate;
	
	void Awake() {
		Instance = this;
		DontDestroyOnLoad(gameObject);

		SpawnPoints = GetComponentsInChildren<CustomerSpawn>();
	}

	public void SpawnNextCustomer() {
		doingThings = true;
		
		var obj = Instantiate(CustomerTemplate, transform.position, Quaternion.AngleAxis(60, Vector3.right)) as GameObject;
		currentCustomer = obj.GetComponent<Customer>();
		
		iTween.MoveTo(currentCustomer.gameObject, iTween.Hash("position", windowCenter,
		                                                      "easeType", "easeInOut",
		                                                      "time", 3.5f,
		                                                      "oncomplete", "CustomerEnterFinish",
		                                                      "onCompleteTarget", gameObject));
	}
	
	void CustomerEnterFinish() {
		Flow.CurrentPhase = Flow.Phase.CustomerSpeech;
		doingThings = false;
	}
}
