﻿using System;
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

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		bubbleWidth = spriteRenderer.bounds.size.x;
		spriteRenderer.enabled = false;
		RecipeIcons = new List<GameObject>();
	}
	
	public void GenerateRecipe() {
		spriteRenderer.enabled = true;
		CurrentRecipe = Ingredients.Instance.GetRecipe(3, false);
		iTween.PunchScale(gameObject, iTween.Hash("amount", new Vector3(2f,1f,0f),
                                                  "time", 0.8f,
		                                          "oncomplete", "AnimateIngredients",
                                                  "onCompleteTarget", gameObject));
	}

	void AnimateIngredients() {
		foreach (string orderPart in CurrentRecipe) {
			iconPosition = transform.position;
			//iconPosition.x = ((bubbleWidth / (float)CurrentRecipe.Count) * (CurrentRecipe.IndexOf(orderPart) + 1f));
			newRecipeIcon = Instantiate(Ingredients.Instance.SnackTemplate, iconPosition, Quaternion.identity) as GameObject;
			newRecipeIcon.GetComponent<SpriteRenderer>().sprite = Ingredients.Instance.Sprites.Where(i => i.name == orderPart).First();
			newRecipeIcon.GetComponent<SpriteRenderer>().sortingOrder = 2;
			newRecipeIcon.transform.SetParent(gameObject.transform);
			iTween.PunchScale(newRecipeIcon, iTween.Hash("amount", new Vector3(1f,1f,0f),
			                                          	 "time", 0.3f));
			RecipeIcons.Add(newRecipeIcon);
		}
		Customers.Instance.CustomerOrderFinish();
	}
}
