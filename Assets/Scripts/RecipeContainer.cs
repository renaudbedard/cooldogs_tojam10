using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RecipeContainer : MonoBehaviour {

	List<string> CurrentRecipe;
	SpriteRenderer spriteRenderer;
	List<GameObject> RecipeIcons;
	GameObject newRecipeIcon;
	float bubbleWidth;
	Vector3 iconPosition;

	IngredientSpawn[] SpawnPoints;
	public Transform SpawnPointsContainer;

	public Silhouette Silhouette;
	public CookingMat CookingMat;

	public float Snackiness;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.enabled = false;
		RecipeIcons = new List<GameObject>();
		SpawnPoints = SpawnPointsContainer.GetComponentsInChildren<IngredientSpawn>();
	}
	
	public void GenerateRecipe() {
		foreach (GameObject icon in RecipeIcons) {
			Destroy(icon);
		}
		spriteRenderer.enabled = true;
		bubbleWidth = spriteRenderer.bounds.size.x;
		CurrentRecipe = Ingredients.Instance.GetRecipe(UnityEngine.Random.Range(2, 6), Customers.Instance.CurrentEatsGarbage());
		iTween.PunchScale(gameObject, iTween.Hash("amount", new Vector3(2f,1f,0f),
                                                  "time", 0.8f,
		                                          "oncomplete", "AnimateIngredients",
                                                  "onCompleteTarget", gameObject));

		var ingredients = Ingredients.Instance.GetIngredients(SpawnPoints.Length, CurrentRecipe, Snackiness);
		int i = 0;
		foreach (var spawnPoint in SpawnPoints.Shuffle())
			spawnPoint.Spawn(ingredients[i++]);

		Snackiness = Mathf.Clamp01(Snackiness + 0.1f);

		// randomize silhouette
		Silhouette.Randomize();
		CookingMat.SpawnNewPlate();

		Customers.Instance.CustomerTalk();
	}

	public bool RateRecipe()
	{
		float silhouetteMatch = Silhouette.TestMatch();
		float ingredientsMatch = Ingredients.Instance.GetScore(CurrentRecipe, FindObjectsOfType<Snack>().Where(x => x.placed).Select(x => x.snackType).ToList());

		bool win = ingredientsMatch >= 1 && silhouetteMatch > Silhouette.FailThreshold;
		Debug.Log("Ingredients : " + ingredientsMatch + ", Silhouette : " + silhouetteMatch + " : WIN? " + win);

		return win;
	}

	void AnimateIngredients() {
		int orderIndex = 0;
		string orderPart;
		for (orderIndex = 0; orderIndex < CurrentRecipe.Count; orderIndex++) {
			orderPart = CurrentRecipe[orderIndex];
			iconPosition = transform.position;
			iconPosition.x += ((bubbleWidth / ((float)CurrentRecipe.Count)) * orderIndex) - (bubbleWidth/2f) + ((bubbleWidth / (float)CurrentRecipe.Count) / 2f);
			newRecipeIcon = Instantiate(Ingredients.Instance.SnackTemplate, iconPosition, Quaternion.identity) as GameObject;
			newRecipeIcon.GetComponentInChildren<SpriteRenderer>().sprite = Ingredients.Instance.Sprites.Where(i => i.name == orderPart).First();
			newRecipeIcon.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;
			newRecipeIcon.GetComponentInChildren<SpriteRenderer>().enabled = false;
			newRecipeIcon.gameObject.layer = gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
			newRecipeIcon.transform.localRotation = Quaternion.identity;
			newRecipeIcon.transform.GetChild(0).localRotation = Quaternion.identity;
			newRecipeIcon.transform.localScale /= 2f;
			newRecipeIcon.transform.SetParent(gameObject.transform);
			iTween.PunchScale(newRecipeIcon, iTween.Hash("amount", new Vector3(1f,1f,1f),
			                                          	 "time", 0.3f,
			                                             "delay", orderIndex/2f,
			                                             "onstart", "EnableIconRenderer",
			                                             "onstarttarget", gameObject,
			                                             "onstartparams", newRecipeIcon));
			RecipeIcons.Add(newRecipeIcon);
		}
	}

	void EnableIconRenderer(GameObject icon)
	{
		GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.9f, 1.1f);
		GetComponent<AudioSource>().Play();

		icon.GetComponentInChildren<SpriteRenderer>().enabled = true;
		if (RecipeIcons.IndexOf (icon) >= RecipeIcons.Count - 1) {
			Customers.Instance.CustomerOrderFinish();
		}
	}

	public void DestroyRecipe() {
		foreach (GameObject icon in RecipeIcons) {
			Destroy(icon);
		}
		spriteRenderer.enabled = false;
	}
}
