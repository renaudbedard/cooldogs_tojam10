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
		
		var obj = Instantiate(CustomerTemplates[UnityEngine.Random.Range(0, CustomerTemplates.Length)], currentSpawnPath.nodes.First(), Quaternion.identity) as GameObject;
		currentCustomer = obj.GetComponent<Customer>();

		iTween.MoveTo(currentCustomer.gameObject, iTween.Hash("path", currentSpawnPath.nodes.ToArray(),
		                                                      "easeType", "easeOutQuad",
		                                                      "time", 2f,
		                                                      "oncomplete", "GenerateRecipe",
		                                                      "onCompleteTarget", recipeContainer.gameObject));
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

	public void ServeCustomer()
	{
		bool satisfied = recipeContainer.RateRecipe();
		recipeContainer.DestroyRecipe();
	}

	public bool CurrentEatsGarbage()
	{
		return currentCustomer.name.StartsWith("Cooldog10");
	}
}
