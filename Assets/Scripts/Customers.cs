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
	string lastCustomerName;

	public Sprite[] Sprites;
	private iTweenPath[] SpawnPaths;
	
	[SerializeField]
	Transform windowCenter;

	[SerializeField]
	GameObject[] CustomerTemplates;

	[SerializeField]
	SkyColours sky;

	[SerializeField]
	RecipeContainer recipeContainer;
	
	void Awake() {
		Instance = this;
		DontDestroyOnLoad(gameObject);

		SpawnPaths = GetComponentsInChildren<iTweenPath>();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			SpawnNextCustomer();
			sky.ChangeColour();
		}
	}

	public void SpawnNextCustomer() {
		doingThings = true;

		DestroyCurrentCustomer();

		currentSpawnPath = SpawnPaths.Shuffle().First();

		string newCustomerName;
		GameObject customerTemplate;
		do
		{
			customerTemplate = CustomerTemplates[UnityEngine.Random.Range(0, CustomerTemplates.Length)];
			newCustomerName = customerTemplate.name;
		} while (newCustomerName == lastCustomerName);

		var obj = Instantiate(customerTemplate, currentSpawnPath.nodes.First(), Quaternion.identity) as GameObject;
		currentCustomer = obj.GetComponent<Customer>();
		lastCustomerName = customerTemplate.name;

		System.Collections.Hashtable iTweenHash = iTween.Hash("path", currentSpawnPath.nodes.ToArray(),
								                              "easeType", "easeOutQuad",
								                              "time", 2f,
								                              "oncomplete", "GenerateRecipe",
								                         	  "onCompleteTarget", recipeContainer.gameObject);

		if (currentSpawnPath.pathName.Contains("orient")) {
			currentCustomer.transform.Rotate(Vector3.forward, 90);
		}

		iTween.MoveTo (currentCustomer.gameObject, iTweenHash);
	}
	
	public void CustomerOrderFinish() {
		Flow.CurrentPhase = Flow.Phase.CustomerSpeech;
		doingThings = false;
	}



	public void DestroyCurrentCustomer() {
		if (currentCustomer != null) {
			Destroy (currentCustomer.gameObject);
		}
	}

	public void ServeCustomer(bool hasSnacks) {
		if (hasSnacks) {
			bool satisfied = recipeContainer.RateRecipe ();
			recipeContainer.DestroyRecipe ();
			Flow.CurrentPhase = Flow.Phase.GiveOut;
		} else {
			iTween.PunchScale(recipeContainer.Silhouette.gameObject, iTween.Hash("amount", new Vector3(1f,1f,0f),
							                                          "time", 0.2f));
		}
	}

	public bool CurrentEatsGarbage()
	{
		return currentCustomer.LikesGarbage;
	}

	public void CustomerTalk()
	{
		currentCustomer.Talk();
	}
	public void CustomerOuch()
	{
		currentCustomer.Ouch();
	}
}
