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
		CurrentRecipe = Ingredients.Instance.GetRecipe(UnityEngine.Random.Range(2, 6), false);
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
	}

	public bool RateRecipe()
	{
		//float silhouetteMatch = Silhouette.TestMatch();
		//float ingredientsMatch = Ingredients.Instance.GetScore(CurrentRecipe, FindObjectsOfType<Snack>().Where(x => x.placed).Select(x => x.snackType).ToList());

		//Debug.Log("Ingredients : " + ingredientsMatch + ", Silhouette : " + silhouetteMatch);

		// todo
		return true;
	}

	void AnimateIngredients() {
		int orderIndex = 0;
		string orderPart;
		for (orderIndex = 0; orderIndex < CurrentRecipe.Count; orderIndex++) {
			orderPart = CurrentRecipe[orderIndex];
			iconPosition = transform.position;
			iconPosition.x += ((bubbleWidth / ((float)CurrentRecipe.Count)) * orderIndex) - (bubbleWidth/2f) + ((bubbleWidth / (float)CurrentRecipe.Count) / 2f);
			newRecipeIcon = Instantiate(Ingredients.Instance.SnackTemplate, iconPosition, Quaternion.identity) as GameObject;
			newRecipeIcon.GetComponent<SpriteRenderer>().sprite = Ingredients.Instance.Sprites.Where(i => i.name == orderPart).First();
			newRecipeIcon.GetComponent<SpriteRenderer>().sortingOrder = 2;
			newRecipeIcon.GetComponent<SpriteRenderer>().enabled = false;
			newRecipeIcon.gameObject.layer = gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
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
		Customers.Instance.CustomerOrderFinish();
	}

	void EnableIconRenderer(GameObject icon){
		icon.GetComponent<SpriteRenderer>().enabled = true;
	}

	public void DestroyRecipe() {
		foreach (GameObject icon in RecipeIcons) {
			Destroy(icon);
		}
		spriteRenderer.enabled = false;
	}
}
